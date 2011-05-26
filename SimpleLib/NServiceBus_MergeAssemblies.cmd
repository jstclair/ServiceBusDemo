REM Assumptions:
REM 1. ILMERGE.EXE in path
REM 2. You are currently in the folder of the assemblies to be merged
REM Usage (all on one line):
REM ILMERGE.EXE						' merging program
REM 	/T:library					' output type (library|exe (console)|winexe (winForms))
REM 	/COPYATTRS					' copy attributes in the merged assemblies to the output assembly
REM 	/OUT:NServiceBusAll.dll		' output filename
REM 	NServiceBus.dll				' primary assembly, followed by other assemblies to be merged
REM 	antlr.runtime.dll Common.Logging.dll Interop.MSMQ.dll NServiceBus.Saga.dll NServiceBus.Serialization.dll ...

ILMERGE.EXE /T:library /COPYATTRS /OUT:NServiceBusAll.dll NServiceBus.dll antlr.runtime.dll Common.Logging.dll Interop.MSMQ.dll NServiceBus.Saga.dll NServiceBus.Serialization.dll NServiceBus.Serializers.Binary.dll NServiceBus.Serializers.Configure.dll NServiceBus.Serializers.XML.dll NServiceBus.Unicast.Config.dll NServiceBus.Unicast.dll NServiceBus.Unicast.Subscriptions.dll NServiceBus.Unicast.Subscriptions.Msmq.dll NServiceBus.Unicast.Transport.dll NServiceBus.Unicast.Transport.Msmq.dll ObjectBuilder.dll ObjectBuilder.SpringFramework.dll Spring.Aop.dll Spring.Core.dll Utils.dll