TwilioEmulator
==============

A Windows C# GUI Twilio Emulator - Designed to make local development of Twilio Applications easier.

The goal is to emulate most of Twilio's functions - including Call control API and basic TWIML Responses , so that you can focus on your call application on your desktop and not have to worry about the cost , and how to get Twilio traffic back to your desktop


![Main Screen](https://raw.github.com/mbadler/TwilioEmulator/master/ScreenShots/MainScreen.png)


__Latest Changes__

[Previous Changes](https://github.com/mbadler/TwilioEmulator/blob/master/README.md#change-log)


Connecting from your application
--------------------------------
The standard Twilio client does not allow for connections other than the real twilio service. It also forces you to provide the credentials every time you create the client. I have created a companion project called [TwilioDotConfig] (https://github.com/mbadler/TwilioDotConfig) that provides for web/app.config settings to auto configure and redirect the standard twilio client.


August 18
  - Call history log window Added - show the history and stauses of all calls
  - Accurate call status for inbound calls


Current Status:
--------------

  - Functionality is currently fully implemeted for Outbound calls.
  - SMS implemented.
  - Basic Incoming Call Support
 
__Planning__  
  - Planning for Full Phone number purchasing API

Down the road:
---------------
- Call list API
- Multiple Phones
- Script phone clients (that respond with scripted digits)
- Headless mode configurable via API call for unit testing external Twilio based applications
- Playing voice files 
- Using Windows TTS to play the actual `SAY` sentences so that you can approximate what the call will sound like 


How it works:
-------------

TwilioEmulator is a windows application that implements a http webserver (WebAPI) to emulate the Twilio Voice Services.
When a web application sends a connect call to the API the emulated phone keypad allows the tester to interact with the emulator.
The emulator requests Twiml Documents from the server and exeutes them to the emulated phone. it listens to responses from the touch tone pad and relays those back to the server.
The server can control the call the executing by calling the redirect or hangup apis.
The server receives a call back when the phone call ends or the phone call could not connect
The phone can be set to Machine mode and the `IfMachine` parameter is honored

Incoming phone calls:
----------------------
Incoming phone calls are being work. Currently any call from the touchpad will go to the default Incoming Phone Number and the default Voice URL.
You specify the defualt number and url in the configuration file or by calling the `AddIncomingPhoneNumber` API call


Outbound SMS Support
--------------------
When a application calls one of the SMS API's (SMS or ShortCode) or returns the SMS verb in Twiml a message is displayed on the phonelog
If a callback was included then the callback is called , The twiml verb can also include a action parameter that will request new
TWiml and execute that.

SMS request will alwys return `SmsStatus=sent`

Twiml Verbs Support:
-------------------------------

The following Twiml are planned for support :



| Verb | Current Status | Description | Remarks |
| --- | --- | --- | --- |
|`Say`| Implemented | Read text to the caller | via output to the log file or perhaps TTS - loop ignored for now|
|`Play`| Implemented | Play an audio file for the caller  | via a message on the log stating that the file would be palyed , maybe in the future we will actually download the file and play it over the speaker - loop ignored for now |
| `Gather` | Implemented | Collect digits the caller types on their keypad | action url's must be absolute  |
| `Sms` | Implemented | Send an SMS message during a phone call | A message displayed in the log will indicate it was sent |
| `Hangup` | Implemented | Hang up the call | |
| `Redirect` | Implemented | Redirect call flow to a different TwiML document. | |
| `Pause` | Implemented | Wait before executing more instructions | |
| `Reject` | In Progress | Decline an incoming call without being billed. | |


API Call Support
-------------------------------

The following API Calls are currently supported or are planned:

| .Net Call | Rest | Status |
| --- | --- | --- |
| `InitiateOutboundCall` | POST /2010-04-01/Accounts/{AccountSid}/Calls | Implemented |
| `GetCall` | GET /2010-04-01/Accounts/{AccountSid}/Calls/{CallSid} | Implemeted |
| `HangupCall` | POST /2010-04-01/Accounts/{AccountSid}/Calls/{CallSid}  "Status" | Impleneted (only Status=completed) |
| `RedirectCall` | POST /2010-04-01/Accounts/{AccountSid}/Calls/{CallSid} "URL" | Implemented |
| `SendSmsMessage` | POST /2010-04-01/Accounts/{AccountSid}/SMS/Messages | Implemented |
| `AddIncomingPhoneNumber` | POST 2010-04-01/Accounts/{AccountSid}/IncomingPhoneNumbers | Planning |


#### Concurrent Phone Calls: ####


  The emulator is built to support many concurrent calls to many differnt numbers at the same time, however for the time bing
  there is only one phone active - the touch tone looking thing on the main form , so you can only have one call going at a time.
  If you make a new call request while a previous one is active you will receive a "BUSY" respone
  
  

__Manual Phone Statuses:__


  You can manually set the status you want call requests to return from the toolbar
  


### Current Known And/or Planned Limitations ###

  - Callbacks are POST only
  - No security checking at all
  - Not all Twiml verbs are supported - see above


### Log ###

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

  
##change log##
August 13
  - Inbound calls from the dial pad now work - click on the dial button on the pad and the call will start, specify the voiceurl in the settings 


August 12:
  - New Http Log Tab , Shows requests received to the API at a Http Level and the results.
  - SMS supported , SMS Api calls and SMS verb in Twiml.
  - Ground work for inbound calls and Emulator control API.
