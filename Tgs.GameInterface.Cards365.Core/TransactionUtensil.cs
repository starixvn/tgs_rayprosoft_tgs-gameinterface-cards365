namespace Tgs.GameInterface.Cards365.Core;

internal class TransactionUtensil : ITransactionUtensil
{
    public string Base64ConvertTransactionId(string source)
    {
        source = source.Replace("_", "/").Replace("-", "+");
        byte[] buffer = Convert.FromBase64String(source + "==");
        return new Guid(buffer).ToString("N");
    }

    public string TransactionIdConvertBase64(string source)
    {
        var transactionGuid = Guid.Parse(source);
        string encoded = Convert.ToBase64String(transactionGuid.ToByteArray());
        encoded = encoded.Replace("/", "_").Replace("+", "-");
        return encoded[..22];
    }
}
