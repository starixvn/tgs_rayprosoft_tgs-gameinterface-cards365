using Tgs.GameInterface.Cards365.Core.Models;

namespace Tgs.GameInterface.Cards365.Core.UnitTest;

public class GameRecordUtensilUnitTest
{
    [Fact]
    public void Status_Is_Bet()
    {
        var transactionInfo = new TransactionInfo(
            "6e91c688ea4011ed853f5254002237ea",
            549,
            "4ucmqplxb029",
            "Bet",
            DateTimeOffset.Parse("2023-05-04T13:56:44.962+08:00"),
            DateTimeOffset.Parse("2023-05-04T13:56:44.886+08:00"),
            DateTimeOffset.UnixEpoch,
            0,
            0,
            0,
            0,
            "{\"txId\":\"6e91c688ea4011ed853f5254002237ea\",\"gameNumber\":\"549-1683179782-585257-1-36\",\"userId\":\"4ucmqplxb029\",\"extension1\":\"cardp101\",\"status\":\"Bet\",\"tiles\":null,\"gameId\":549,\"chair\":0,\"playId\":1,\"commission\":0.0,\"banker\":0,\"profit\":0.0,\"validbet\":0.0,\"bonus\":0.0,\"reward\":0.0,\"outMoney\":0.0,\"currency\":\"CNY\",\"updateTime\":\"05/04/2023 13:56:44.962\",\"createTime\":\"05/04/2023 13:56:44.962\",\"betTransTime\":\"05/04/2023 13:56:44.886\",\"payTransTime\":\"\",\"betAmount\":175.73,\"realBetAmount\":0.0,\"payAmount\":0.0,\"realPayAmount\":0.0}"
            );

        var utensil = new GameRecordUtensil();

        var actual = utensil.TransactionConvertGameRecord(transactionInfo);

        Assert.NotNull(actual);
        Assert.Equal("6e91c688ea4011ed853f5254002237ea", actual.ExternalRoundId);
        Assert.Equal("549", actual.ExternalGameId);
        Assert.Equal("4ucmqplxb029", actual.PlayerCode);
        Assert.False(actual.IsClosed);
        Assert.False(actual.IsVoid);
        Assert.Equal(DateTimeOffset.Parse("2023-05-04T13:56:44.962+08:00"), actual.UpdateTime);
        Assert.Equal(DateTimeOffset.Parse("2023-05-04T13:56:44.886+08:00"), actual.ExternalCreatedOn);
        Assert.Equal(DateTimeOffset.UnixEpoch, actual.ExternalClosedOn);
        Assert.Equal(0.0m, actual.Bet);
        Assert.Equal(0.0m, actual.Validbet);
        Assert.Equal(transactionInfo.Winloss + transactionInfo.Validbet, actual.Settle);
        Assert.Equal(0.0m, actual.Winloss);
        Assert.Equal(0.0m, actual.Commission);
    }

    [Fact]
    public void Status_Is_Pay_Loss()
    {
        var transactionInfo = new TransactionInfo(
            "87032f02ea3d11ed853f5254002237ea",
            549,
            "4ucmqplxb029",
            "Pay",
            DateTimeOffset.Parse("2023-05-04T13:36:16.185+08:00"),
            DateTimeOffset.Parse("2023-05-04T13:35:54.886+08:00"),
            DateTimeOffset.Parse("2023-05-04T13:36:16.092+08:00"),
            33.0m,
            33.0m,
            -33.0m,
            0,
            "{\"txId\":\"87032f02ea3d11ed853f5254002237ea\""
            );

        var utensil = new GameRecordUtensil();

        var actual = utensil.TransactionConvertGameRecord(transactionInfo);

        Assert.NotNull(actual);
        Assert.Equal("87032f02ea3d11ed853f5254002237ea", actual.ExternalRoundId);
        Assert.Equal("549", actual.ExternalGameId);
        Assert.Equal("4ucmqplxb029", actual.PlayerCode);
        Assert.True(actual.IsClosed);
        Assert.False(actual.IsVoid);
        Assert.Equal(DateTimeOffset.Parse("2023-05-04T13:36:16.185+08:00"), actual.UpdateTime);
        Assert.Equal(DateTimeOffset.Parse("2023-05-04T13:35:54.886+08:00"), actual.ExternalCreatedOn);
        Assert.Equal(DateTimeOffset.Parse("2023-05-04T13:36:16.092+08:00"), actual.ExternalClosedOn);
        Assert.Equal(33.0m, actual.Bet);
        Assert.Equal(33.0m, actual.Validbet);
        Assert.Equal(transactionInfo.Winloss + transactionInfo.Validbet, actual.Settle);
        Assert.Equal(-33.0m, actual.Winloss);
        Assert.Equal(0.0m, actual.Commission);
    }

    [Fact]
    public void Status_Is_Pay_Win()
    {
        var transactionInfo = new TransactionInfo(
            "0cede24cea1b11ed853f5254002237ea",
            549,
            "4ucmqplxb029",
            "Pay",
            DateTimeOffset.Parse("2023-05-04T09:29:31.476+08:00"),
            DateTimeOffset.Parse("2023-05-04T09:29:09.699+08:00"),
            DateTimeOffset.Parse("2023-05-04T09:29:31.358+08:00"),
            5.0m,
            4.85m,
            4.85m,
            0.15m,
            "{\"txId\":\"0cede24cea1b11ed853f5254002237ea\""
            );

        var utensil = new GameRecordUtensil();

        var actual = utensil.TransactionConvertGameRecord(transactionInfo);

        Assert.NotNull(actual);
        Assert.Equal("0cede24cea1b11ed853f5254002237ea", actual.ExternalRoundId);
        Assert.Equal("549", actual.ExternalGameId);
        Assert.Equal("4ucmqplxb029", actual.PlayerCode);
        Assert.True(actual.IsClosed);
        Assert.False(actual.IsVoid);
        Assert.Equal(DateTimeOffset.Parse("2023-05-04T09:29:31.476+08:00"), actual.UpdateTime);
        Assert.Equal(DateTimeOffset.Parse("2023-05-04T09:29:09.699+08:00"), actual.ExternalCreatedOn);
        Assert.Equal(DateTimeOffset.Parse("2023-05-04T09:29:31.358+08:00"), actual.ExternalClosedOn);
        Assert.Equal(5m, actual.Bet);
        Assert.Equal(4.85m, actual.Validbet);
        Assert.Equal(transactionInfo.Winloss + transactionInfo.Validbet, actual.Settle);
        Assert.Equal(4.85m, actual.Winloss);
        Assert.Equal(0.15m, actual.Commission);
    }
}
