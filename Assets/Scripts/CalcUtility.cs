/// <summary>
/// 計算用ユーティリティクラス
/// </summary>
public class CalcUtility
{
    /// <summary>
    /// 指定した小数点の桁数になるように四捨五入した値を戻す
    /// </summary>
    /// <param name="value">計算する値</param>
    /// <param name="digitsNum">残したい小数点の桁数(この１つ下の値を四捨五入する)</param>
    /// <returns></returns>
    public static float CalculateRoundHalfUp(float value, int digitsNum) {
        return float.Parse(value.ToString("F" + digitsNum.ToString()));
    }
}
