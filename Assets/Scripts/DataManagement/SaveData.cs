/// セーブデータの実体クラス
/// float型やUnity独自の型は使えず
/// int、double、string、boolだけ使えると覚えておいてください
/// 特にfloatが使えないのは忘れがちなので注意
/// ネストしたオブジェクト型が使えるかは未検証
///
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    //各ステージのアンロック演出の有無
    public int[] stagesStatus = new int[(int)Stage.Tutorial];

    //チュートリアルステージのビット演算に使用
    public int tutorialStatusBit;

    //下記は書き方の例です。
    //public int a = 0;
    //public string b = "string";
    //public double c = 3.14d;
    //public bool d = false;
}
