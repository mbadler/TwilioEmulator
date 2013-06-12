TwilioEmulator
==============

A Windows C# GUI Twilio Emulator - Designed to make local development of Twilio Applications easier.

The goal is to emulate most of Twilio's functions - including Call control API and basic TWIML Responses , so that you can focus on your call application on your desktop and not have to worry about the cost , and how to get Twilio traffic back to your desktop


Current Status
--------------

  - The emulator listens fro REST Connections on port 28080
  - POSTS to calls.json (twilio.InitiateOutboundCall() with the c# helper):
    - Request are excepted
    - A response indicating that the call has been queued is returned
    - The call is added to the call queue
  - All requests and responses are logged




**__For developers using the .Net helper:__**
The twilio rest client has the address to the twilio servers hard coded. To acess the emulator on local host you can use the TwilioTestClient.cs class which uses reflection to change the underlying address

  
