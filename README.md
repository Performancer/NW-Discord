# nw-mongo-database

API
- Logs game data such as chat messages, logins, deaths and in-game announcements
(things that the server does not want to keep track of by itself, but is easier to access via an API than with a local log file)

Authentication
- This API is only accessible with a secret authorization key (or in discord in a private server admininstrator channel)

Discord
- Discord Event Feed for server administrators (sent to a private channel)
- Queries from the database through an assigned query channel in Discord
- In-game announcements to a public Discord channel of the game server community
