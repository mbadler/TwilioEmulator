TwilioEmulator
==============

A Windows C# GUI Twilio Emulator - Designed to make local development of Twilio Applications easier.

The goal is to emulate most of Twilio's functions - including Call control API and basic TWIML Responses , so that you can focus on your call application on your desktop and not have to worry about the cost , and how to get Twilio traffic back to your desktop


Current Status
--------------

  The Emulator is currently focused on outbound initiated phone calls.
  Inbound Phone calls will be added later.

  - The emulator listens fro REST Connections on port 28080
  - POSTS to calls.json (twilio.InitiateOutboundCall() with the c# helper):
    - Request are accepted
    - A response indicating that the call has been queued is returned
    - The call is added to the call queue
    - If the overall call status is set to busy,No-answer etc... then a callback is sent to the status url with that staus
    - The phone key pad starts to blink indicateing that the phone is ringing
    - Click the pickup phone button - the phone call connects
    - A connect request Twiml is sent
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


__Log__

  Logging is currently to a treeview on the main form of the emulator
  Each request is marked with a text symbol to show the direction of the call
  <pre>
    () = www , [] = TwilioEmulator } = phone
  </pre>
  so:
  <pre>
    () --> []  =  Incoming rest API call
    () <-- []  =  Outgoing Twiml request
    [] --> }   =  Request to the phone (dial ,say ) etc...
    [] <-- }   =  phone notifing server (digit keypress, hangup, etc...)
  </pre>

**__For developers using the .Net helper:__**

The twilio rest client has the address to the twilio servers hard coded. To acess the emulator on local host you can use the TwilioTestClient.cs class which uses reflection to change the underlying address

  
Updated 6/18/2013
