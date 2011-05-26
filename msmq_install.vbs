Const MQ_ERROR_QUEUE_EXISTS = -1072824317

set qinfo = CreateObject("MSMQ.MSMQQueueInfo")

CreateQueue ".\private$\messagebus"
CreateQueue ".\private$\subscriptions"
CreateQueue ".\private$\error"

CreateQueue ".\private$\client"
CreateQueue ".\private$\worker"
CreateQueue ".\private$\worker2"
CreateQueue ".\private$\external"

' not required for this demo, but part of nServiceBus samples, and should therefore leave them
CreateQueue ".\private$\distributorStorage"
CreateQueue ".\private$\distributorcontrolbus"
CreateQueue ".\private$\distributordatabus"
CreateQueue ".\private$\timeoutmanager"

Sub DeleteQueue(queueName)
	On Error Resume Next
	qinfo.PathName = queueName
	qinfo.Delete
	If Err.Number <> MQ_ERROR_QUEUE_EXISTS And Err.Number <> 0 Then
		MsgBox "Error " & CStr(Err.Number) & ": " & Err.Description
		WScript.Quit
	End If
End Sub

Sub CreateQueue(queueName)
	DeleteQueue queueName
	qinfo.PathName = queueName
	qinfo.Create true
End Sub
