using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditManager : MonoBehaviour {

	void Start ()
	{
		StartCoroutine(EndCredit());
	}

	IEnumerator EndCredit()
	{
		yield return new WaitForSeconds(60f);
		SceneManager.LoadScene("Main");
	}

	public void LoadMain()
	{
		SceneManager.LoadScene("Main");
	}

}
