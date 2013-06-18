TwilioEmulator
==============

A Windows C# GUI Twilio Emulator - Designed to make local development of Twilio Applications easier.

The goal is to emulate most of Twilio's functions - including Call control API and basic TWIML Responses , so that you can focus on your call application on your desktop and not have to worry about the cost , and how to get Twilio traffic back to your desktop


Current Status
--------------

  - The emulator listens fro REST Connections on port 28080
  - POSTS to calls.json (twilio.InitiateOutboundCall() with the c# helper):
    - Request are accepted
    - A response indicating that the call has been queued is returned
    - The call is added to the call queue
    - A callback is sent back to the indicated url with the status of ringing busy or no-answer
  - All requests and responses are logged




__Concurrent Phone Calls:__


  The emulator is built to support many concurrent calls to many differnt numbers at the same time, however for the time bing
  there is only one phone active - the touch tone looking thing on the main form , so you can only have one call going at a time.
  If you make a new call request while a previous one is active you will receive a "BUSY" respone
  
  

__Manual Phone Statuses:__


  You can manually set the status you want call requests to return from the toolbar
  


__Current Known And/or Planned Limitations__

  - Callbacks are POST only
  - No security checking at all





**__For developers using the .Net helper:__**

The twilio rest client has the address to the twilio servers hard coded. To acess the emulator on local host you can use the TwilioTestClient.cs class which uses reflection to change the underlying address

  
Updated 6/18/2013
