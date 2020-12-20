/***************************************************************************/
/** 	© 2018 NULLcode Studio. All Rights Reserved.
/** 	Разработано в рамках проекта: http://null-code.ru/
/** 	WebMoney: R209469863836, Z126797238132, E274925448496, U157628274347
/** 	Яндекс.Деньги: 410011769316504
/***************************************************************************/

using System.Collections;
using UnityEngine;

public class Match3Node : MonoBehaviour {

	public SpriteRenderer sprite; // спрайт узла
	public GameObject highlight; // объект подсветки узла
	public int id { get; set; }
	public bool ready { get; set; }
	public int x { get; set; }
	public int y { get; set; }
}
