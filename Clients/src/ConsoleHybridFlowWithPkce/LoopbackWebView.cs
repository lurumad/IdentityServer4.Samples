using IdentityModel.OidcClient.WebView;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ConsoleCoreHybridFlowWithPkce
{
    public class LoopbackWebView : IWebView
    {
        private readonly int _port;
        private readonly string _path;

        public LoopbackWebView(int port, string path = null)
        {
            _port = port;
            _path = path;
        }

        public event EventHandler<HiddenModeFailedEventArgs> HiddenModeFailed;

        public async Task<InvokeResult> InvokeAsync(InvokeOptions options)
        {
            using (var listener = new LoopbackHttpListener(_port, _path))
            {
                OpenBrowser(options.StartUrl);

                try
                {
                    var result = await listener.WaitForCallbackAsync();
                    if (String.IsNullOrWhiteSpace(result))
                    {
                        return new InvokeResult { ResultType = InvokeResultType.UnknownError, Error = "Empty response." };
                    }

                    return new InvokeResult { Response = result, ResultType = InvokeResultType.Success };
                }
                catch (TaskCanceledException ex)
                {
                    return new InvokeResult { ResultType = InvokeResultType.Timeout, Error = ex.Message };
                }
                catch (Exception ex)
                {
                    return new InvokeResult { ResultType = InvokeResultType.UnknownError, Error = ex.Message };
                }
            }
        }

        public static void OpenBrowser(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}