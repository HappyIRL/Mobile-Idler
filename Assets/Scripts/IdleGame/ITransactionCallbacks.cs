using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITransactionCallbacks
{
	public void TransactionState(bool valid);
}
