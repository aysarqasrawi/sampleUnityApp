using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class BalanceEquation : MonoBehaviour {
	public InputField firstLeftSideInput;
	public InputField secondLeftSideInput;
	public InputField rightSideInput;
	public string[,] leftSide = new string[2,2];
	string[,] listOfElements = new string[,] {{"H2","g"},{"Ca","g"}, {"C","s"},{"Na","g"}, {"N","s"}, {"O2","s"},{"Mg","g"}, {"Al","s"},{"Fe","s"}};
	string[] equations;
	string[] LeftSide;
	string RightSide;
	bool correctRightSide = false;
	bool correctFirstLeftSide = false;
	bool correctSecondLeftSide = false;

	void Start () {

	}	  	

	public void FillLeftSide(int index, string element){
		leftSide[index,1] =element;
		StartCoroutine (CallOnlineBalanceService());
	}

	void CheckLeftSide(string[] LeftSide){
		string[] LeftSideElements = LeftSide[0].Split ('+');
		string firstLeftSideInputValue, secondLeftSideInputValue;
		firstLeftSideInputValue = firstLeftSideInput.text.Trim();
		secondLeftSideInputValue = secondLeftSideInput.text.Trim();
		if (firstLeftSideInputValue == "") {
			firstLeftSideInputValue = "1";
		} else if (secondLeftSideInputValue == "")
			secondLeftSideInputValue = "1";

		for (int i = 0; i < listOfElements.Length; i++) {
			if (LeftSideElements [0].Contains (listOfElements [i,0].ToString())) {
				print ("Founded!" + i + "," + listOfElements [i,0].ToString());
				string element = string.Concat(listOfElements [i,0].ToString());
				string balanceValue = (LeftSideElements [0].Replace (element,"")).Trim();
				if (balanceValue == "") {
					balanceValue = "1";
				}
				if (firstLeftSideInputValue == balanceValue) {
					correctFirstLeftSide = true;
					ColorBlock cb = firstLeftSideInput.colors;
					cb.normalColor = Color.white;
					firstLeftSideInput.colors = cb;
				} else {
					ColorBlock cb = firstLeftSideInput.colors;
					cb.normalColor = Color.red;
					firstLeftSideInput.colors = cb;
				}
				print ("balanceValue:::"+balanceValue+","+firstLeftSideInput.text);
				break;
			}
		}


		for (int i = 0; i < listOfElements.Length; i++) {
			if (LeftSideElements [1].Contains (listOfElements [i,0].ToString())) {
				print ("Founded!" + i + "," + listOfElements [i,0].ToString());
				string element = string.Concat(listOfElements [i,0].ToString());
				string balanceValue = (LeftSideElements [1].Replace (element,"")).Trim();
				if (balanceValue == "") {
					balanceValue = "1";
				}
				if (secondLeftSideInputValue == balanceValue) {
					correctSecondLeftSide= true;
					ColorBlock cb = secondLeftSideInput.colors;
					cb.normalColor = Color.white;
					secondLeftSideInput.colors = cb;
				} else {
					ColorBlock cb = secondLeftSideInput.colors;
					cb.normalColor = Color.red;
					secondLeftSideInput.colors = cb;
				}
				print ("balanceValue:::"+balanceValue);
				break;
			}
		}

	}


	void CheckRightSide(string RightSide){
		print ("RightSide:"+RightSide);
		if (rightSideInput.text.ToLower() == RightSide.ToLower()) {
			correctRightSide = true;
			ColorBlock cb = rightSideInput.colors;
			cb.normalColor = Color.white;
			rightSideInput.colors = cb;
		} else {
			ColorBlock cb = rightSideInput.colors;
			cb.normalColor = Color.red;
			rightSideInput.colors = cb;
		}
		
	}
	public void CheckResult(){
		CheckLeftSide (LeftSide);
		CheckRightSide (RightSide.Trim());
		PrintResult ();
	}

	void PrintResult(){
		if (correctRightSide && correctSecondLeftSide && correctFirstLeftSide) {
			GameObject.Find ("ResultText").GetComponent<Text> ().text = "YESSSS!!!!";
			GameObject.Find ("ResultText").GetComponent<Text> ().color = Color.green;

		} else {
			GameObject.Find ("ResultText").GetComponent<Text> ().text = "NOOO!!!!";
			GameObject.Find ("ResultText").GetComponent<Text> ().color = Color.red;
		}
	}

	public void viewResult(){
		string[] LeftSideElements = LeftSide[0].Split ('+');
		for (int i = 0; i < listOfElements.Length; i++) {
			if (LeftSideElements [0].Contains (listOfElements [i,0].ToString())) {
				print ("Founded!" + i + "," + listOfElements [i,0].ToString());
				string element = string.Concat(listOfElements [i,0].ToString());
				string balanceValue = (LeftSideElements [0].Replace (element,"")).Trim();
				ColorBlock cb = firstLeftSideInput.colors;
				cb.normalColor = Color.white;
				firstLeftSideInput.colors = cb;
				if (balanceValue == "") {
					balanceValue = "1";
				}
				firstLeftSideInput.text = balanceValue;
				break;
			}
		}


		for (int i = 0; i < listOfElements.Length; i++) {
			if (LeftSideElements [1].Contains (listOfElements [i,0].ToString())) {
				print ("Founded!" + i + "," + listOfElements [i,0].ToString());
				string element = string.Concat(listOfElements [i,0].ToString());
				string balanceValue = (LeftSideElements [1].Replace (element,"")).Trim();
				ColorBlock cb = secondLeftSideInput.colors;
				cb.normalColor = Color.white;
				secondLeftSideInput.colors = cb;
				if (balanceValue == "") {
					balanceValue = "1";
				}
				secondLeftSideInput.text = balanceValue;
				break;
			}

			rightSideInput.text = RightSide.Trim ();
		}
	}

		IEnumerator CallOnlineBalanceService(){
		string url = "http://www.endmemo.com/chem/ajax/balancer_ajax.php?q="+leftSide[0, 1]+"   "+leftSide[1, 1];
		UnityWebRequest  www = UnityWebRequest.Get(url);
		yield return www.SendWebRequest();

		if(www.isNetworkError || www.isHttpError) {
			Debug.Log(www.error);
		}
		else {
			// Show results as text
			string result = www.downloadHandler.text;
			 equations = result.Split('\n');
			Debug.Log(equations[0]);
			LeftSide = equations[0].Split ('=');
			RightSide = LeftSide [1];
			}
			// Or retrieve results as binary data
			byte[] results = www.downloadHandler.data;
			//Debug.Log ("results: "+results);
	}

}
