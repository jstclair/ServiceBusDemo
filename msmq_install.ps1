[Reflection.Assembly]::LoadWithPartialName("System.Messaging")

function Delete-Queue($queueName) {
    if ([System.Messaging.MessageQueue]::Exists($queueName))
    {
        [System.Messaging.MessageQueue]::Delete($queueName)
        Write-Host "Deleting queue $queueName"
    }
}

function Create-Queue($queueName, $userName) {
    $qb = [System.Messaging.MessageQueue]::Create($queueName)
	 if ($userName) {
	    $acl = ([System.Messaging.MessageQueueAccessRights]::GenericRead -bor [System.Messaging.MessageQueueAccessRights]::GenericWrite)
		 $qb.SetPermissions($userName, $acl, [System.Messaging.AccessControlEntryType]::Set)
	 }
}

$queues = ".\private$\messagebus", ".\private$\subscriptions", ".\private$\error", ".\private$\client", ".\private$\worker", ".\private$\worker2", ".\private$\external"
$appPoolUser = "DefaultAppPool" # Win7/2008 + IIS 7.5

$queues | % { Delete-Queue $_; Create-Queue $_ $appPoolUser }