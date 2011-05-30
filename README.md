# REQUIREMENTS #

1. You must have Microsoft Message Queueing (MSMQ) installed on your machine. If running Vista/Windows7, this is probably already installed, but you may have stopped the service. 

2. From an elevated command prompt, run:
a. MSMQ_start.cmd: This starts the MSMQ service.
b. msmq_install.ps1: (Powershell) This will install the required queues for this demo and assign the correct ACLs for IIS 7.5 (and will delete them first if they already exist).


## SCENARIOS ##
1. [As Is]: Run the demo, and send messages by pressing Enter in the OurSystem console.
2. [As Is]: Take down any of the dependent systems (by closing the console, or typing "q" and pressing Enter). Send some messages -- systems still running will process the current messages. Bring the systems back up (by right-clicking the project in VS, and choosing "Debug, Start New Instance"). Messages will be delivered. If you have taken down ExternalSystem, note that replies are delivered on outstanding requests back to TheirSystem.
3. [Durable]: Note the comments in the Messages project. By default, messages are stored in memory; add the attribute in order to guarantee that messages are persisted to disk. For more info, you can see: http://msdn.microsoft.com/en-us/library/ms978430.aspx
4. [Lost messages]: Run the demo, and take down ExternalSystem while it is processing a request (i.e., when it prints the "Sleeping for ... milliseconds" message). This can be easier to do if you do the following first:
	- In ExternalSystem's MissingCustomerInformationMessageHandler.cs file, comment out the line:
	int delay = ConfigUtils.RandomDelay;
	and uncomment the following line:
	int delay = 8000;
	
	This will give you time to close the window while processing a message.
	
	- In TheirSystem's AddressChangeMessageHandler.cs, you may want to comment out the last line:
	repository.AddOrUpdate(info.SSN, info);
	
	This will make sure that no addresses are stored, and all calls will require a lookup from ExternalSystem.

The consequence is that once messages are passed into the handler, they are removed from the message queue regardless of outcome; resulting in lost messages in the case of an application crash. This is resolved by making the queue read transactional:

	- In ExternalSystem's Program.cs, change the following section:
	
	new ConfigMsmqTransport(builder)
                .IsTransactional(false)
                .PurgeOnStartup(false);
                
    to:
    
    new ConfigMsmqTransport(builder)
                .IsTransactional(true)
                .PurgeOnStartup(false);
                
Now run the demo again, take down ExternalSystem while it is processing messages, and then start it again. Note that the message wasn't taken out of the queue, and is handled.

5. [Scaling]: Note: you should probably reset the ExternalSystem's delay back to Random at this point. Start the demo, and point out that you can send a batch of messages by typing a number into OurSystem's console (try "5"). This is similar to the batch requirements we have defined. In the case where, for instance, ExternalSystem has gone down, and we have queued a lot of messages to be processed, our SLA would be difficult to achieve. 

Start the demo, and open a Windows Explorer window to the "bin" directory of ExternalSystem (if you have one of the ExternalSystem files open in VS, right-click on the tab and choose "Open containing folder"). Initiate a send of a large number of messages from OurSystem -- 200 is a good number. While these are processing, navigate back to the "bin" folder and make several copies of ExternalSystem's "Debug" folder (3 or 4). In each of these folders, start the ExternalSystem.exe file. Note that each copy will now collaborate on handling messages -- you can also show that, following the changes in 4, you can take down one process and the others will still pick up the message. Transparent scaling to handle peak loads.

6. [Multi-threaded]: Note that each instance is picking up a single message at a time; this is most apparent when you have only a single copy of ExternalSystem running. You can change this for the various systems involved by opening their "app.config" file and changing:

<MsmqTransportConfig
    InputQueue="worker"
    ErrorQueue="error"
    NumberOfWorkerThreads="1"
    MaxRetries="5"
  />
  
  to:
  
<MsmqTransportConfig
	InputQueue="worker"
	ErrorQueue="error"
	NumberOfWorkerThreads="{some number larger than 1}"
	MaxRetries="5"
/>

Rerun the demo, and note how various systems receive multiple messages before processing them. Again, you can dial up the number to around 20 for ExternalSystem, and then send a batch from OurSystem of 40. Total time to process the messages should not exceed approx. 16 seconds.