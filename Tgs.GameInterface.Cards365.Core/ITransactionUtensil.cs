namespace Tgs.GameInterface.Cards365.Core;

public interface ITransactionUtensil
{
    public string Base64ConvertTransactionId(string source);

    public string TransactionIdConvertBase64(string source);
}
