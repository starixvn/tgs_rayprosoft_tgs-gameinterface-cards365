namespace Tgs.GameInterface.Cards365.Core.UnitTest;

public class TransactionUtensilUnitTest
{
    [Fact]
    public void Base64_Conversion_TransactionId()
    {
        var transactionId = "qERaf790J0mBUrlIKkKvnA";
        var utensil = new TransactionUtensil();
        var actual = utensil.Base64ConvertTransactionId(transactionId);

        Assert.Equal(Guid.Parse("7f5a44a8-74bf-4927-8152-b9482a42af9c").ToString("N"), actual);
    }

    [Fact]
    public void TransactionId_Conversion_Base64()
    {
        var transactionId = Guid.Parse("7f5a44a8-74bf-4927-8152-b9482a42af9c").ToString("N");
        var utensil = new TransactionUtensil();
        var actual = utensil.TransactionIdConvertBase64(transactionId);

        Assert.Equal("qERaf790J0mBUrlIKkKvnA", actual);
    }
}
