/// <summary>
/// �v�Z�p���[�e�B���e�B�N���X
/// </summary>
public class CalcUtility
{
    /// <summary>
    /// �w�肵�������_�̌����ɂȂ�悤�Ɏl�̌ܓ������l��߂�
    /// </summary>
    /// <param name="value">�v�Z����l</param>
    /// <param name="digitsNum">�c�����������_�̌���(���̂P���̒l���l�̌ܓ�����)</param>
    /// <returns></returns>
    public static float CalculateRoundHalfUp(float value, int digitsNum) {
        return float.Parse(value.ToString("F" + digitsNum.ToString()));
    }
}
