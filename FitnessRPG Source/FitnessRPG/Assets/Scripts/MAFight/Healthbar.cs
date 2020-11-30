using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour {

	public Image currentHealthbar;
	public Text ratioText;

	private float hitPoint = 150;
	private float maxHitPoint = 150;

	private void Start()
	{
		updateHealthbar ();
	}

	private void updateHealthbar()
	{
		float hitRatio = hitPoint / maxHitPoint;
		currentHealthbar.rectTransform.localScale = new Vector3 (hitRatio, 1, 1);
		ratioText.text = (hitRatio * 100).ToString ("0") + '%';
	}

	private void takeDamage(float damage)
	{
		hitPoint -= damage;
		if (hitPoint <= 0) 
		{
			hitPoint = 0;
			// destroy object routine
		} 
		updateHealthbar ();
	}

	private void healDamage(float heal)
	{
		hitPoint += heal;
		if (hitPoint >= maxHitPoint) 
		{
			hitPoint = maxHitPoint;
		} 
		updateHealthbar ();
	}
}
