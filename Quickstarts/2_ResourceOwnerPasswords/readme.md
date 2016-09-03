# Quickstart #2: Securing an API using the Resource Owner Password Grant

This quickstart adds support for the OAuth 2.0 resource owner password grant. 
This allows a client to send a user's name and password to identityserver to request a token representing that user.


**note** The resource owner password grant is only recommended for so called "trusted clients" - in many cases you are better off with an OpenID Connect based flow for user authentication.
Nevertheless, this sample allows for an easy way to introduce users in identityserver - that's why we included it.
