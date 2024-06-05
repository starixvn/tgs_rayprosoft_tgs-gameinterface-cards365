using Tgs.GameInterface.Cards365.Core.Models;

namespace Tgs.GameInterface.Cards365.Core.UnitTest;

public class GameWagerUtensilUnitTest
{
	[Fact]
	public void Status_Is_Bet()
	{
		var transactionInfo = new TransactionInfo(
			"6e91c688ea4011ed853f5254002237ea",
			"549-1683179782-585257-1-36",
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

		var utensil = new GameWagerUtensil();

		var actual = utensil.TransactionConvertGameWager(transactionInfo);

		Assert.NotNull(actual);
		Assert.Equal("549-1683179782-585257-1-36", actual.ExternalRoundId);
		Assert.Equal("6e91c688ea4011ed853f5254002237ea", actual.ExternalWagerId);
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
			TxId: "bd606f6c6d0811eebfe04cd98f535a89",
			GameNumber: "449-1697559479-532463-1",
			GameId: 449,
			UserId: "4ujhc2phm6bd",
			Status: "Pay",
			UpdateTime: DateTimeOffset.Parse("2023-10-18T00:18:30.157+08:00"),
			BetTransTime: DateTimeOffset.Parse("2023-10-18T00:18:06.892+08:00"),
			PayTransTime: DateTimeOffset.Parse("2023-10-18T00:18:30.072+08:00"),
			Bet: 28.6m,
			Validbet: 28.6m,
			Winloss: -28.6m,
			Commission: 0,
			AdditionalData: "{\"txId\":\"bd606f6c6d0811eebfe04cd98f535a89\",\"gameNumber\":\"449-1697559479-532463-1\",\"userId\":\"4ujhc2phm6bd\",\"extension1\":\"cardp121\",\"status\":\"Pay\",\"tiles\":\"[a1, a3, a4, a5, a6, a7, a8, a8, a9, d5, d5, e4, e4]\",\"gameId\":449,\"chair\":0,\"playId\":1,\"commission\":0,\"banker\":0,\"profit\":-28.6,\"validbet\":28.6,\"bonus\":0,\"reward\":0,\"outMoney\":0,\"currency\":\"HKD\",\"updateTime\":\"10/18/2023 00:18:30.157\",\"createTime\":\"10/18/2023 00:18:06.952\",\"betTransTime\":\"10/18/2023 00:18:06.892\",\"payTransTime\":\"10/18/2023 00:18:30.072\",\"betAmount\":1310.99,\"realBetAmount\":28.6,\"payAmount\":1282.39,\"realPayAmount\":0}"
			);

		var utensil = new GameWagerUtensil();

		var actual = utensil.TransactionConvertGameWager(transactionInfo);

		Assert.NotNull(actual);
		Assert.Equal("449-1697559479-532463-1", actual.ExternalRoundId);
		Assert.Equal("bd606f6c6d0811eebfe04cd98f535a89", actual.ExternalWagerId);
		Assert.Equal("449", actual.ExternalGameId);
		Assert.Equal("4ujhc2phm6bd", actual.PlayerCode);
		Assert.True(actual.IsClosed);
		Assert.False(actual.IsVoid);
		Assert.Equal(DateTimeOffset.Parse("2023-10-18T00:18:30.157+08:00"), actual.UpdateTime);
		Assert.Equal(DateTimeOffset.Parse("2023-10-18T00:18:06.892+08:00"), actual.ExternalCreatedOn);
		Assert.Equal(DateTimeOffset.Parse("2023-10-18T00:18:30.072+08:00"), actual.ExternalClosedOn);
		Assert.Equal(28.6m, actual.Bet);
		Assert.Equal(28.6m, actual.Validbet);
		Assert.Equal(transactionInfo.Winloss + transactionInfo.Validbet, actual.Settle);
		Assert.Equal(-28.6m, actual.Winloss);
		Assert.Equal(0.0m, actual.Commission);
	}

	[Fact]
	public void Status_Is_Pay_Win()
	{
		var transactionInfo = new TransactionInfo(
			"d048f1806d0811eebfe04cd98f535a89",
			"449-1697559511-395695-1",
			449,
			"4ujhc2phm6bd",
			"Pay",
			DateTimeOffset.Parse("2023-10-18T00:19:40.650+08:00"),
			DateTimeOffset.Parse("2023-10-18T00:18:38.090+08:00"),
			DateTimeOffset.Parse("2023-10-18T00:19:40.568+08:00"),
			60.82m,
			62.7m,
			60.82m,
			1.88m,
			"{\"txId\":\"d048f1806d0811eebfe04cd98f535a89\",\"gameNumber\":\"449-1697559511-395695-1\",\"userId\":\"4ujhc2phm6bd\",\"extension1\":\"cardp121\",\"status\":\"Pay\",\"tiles\":\"[a8, a8, a9, a9, d1, d1, d5, d5, d9, d9, e4, e4, e6, e6]\",\"gameId\":449,\"chair\":0,\"playId\":1,\"commission\":1.88,\"banker\":1,\"profit\":60.82,\"validbet\":60.82,\"bonus\":0,\"reward\":0,\"outMoney\":0,\"currency\":\"HKD\",\"updateTime\":\"10/18/2023 00:19:40.650\",\"createTime\":\"10/18/2023 00:18:38.150\",\"betTransTime\":\"10/18/2023 00:18:38.090\",\"payTransTime\":\"10/18/2023 00:19:40.568\",\"betAmount\":1282.39,\"realBetAmount\":62.7,\"payAmount\":1343.21,\"realPayAmount\":62.7}"
			);

		var utensil = new GameWagerUtensil();

		var actual = utensil.TransactionConvertGameWager(transactionInfo);

		Assert.NotNull(actual);
		Assert.Equal("449-1697559511-395695-1", actual.ExternalRoundId);
		Assert.Equal("d048f1806d0811eebfe04cd98f535a89", actual.ExternalWagerId);
		Assert.Equal("449", actual.ExternalGameId);
		Assert.Equal("4ujhc2phm6bd", actual.PlayerCode);
		Assert.True(actual.IsClosed);
		Assert.False(actual.IsVoid);
		Assert.Equal(DateTimeOffset.Parse("2023-10-18T00:19:40.650+08:00"), actual.UpdateTime);
		Assert.Equal(DateTimeOffset.Parse("2023-10-18T00:18:38.090+08:00"), actual.ExternalCreatedOn);
		Assert.Equal(DateTimeOffset.Parse("2023-10-18T00:19:40.568+08:00"), actual.ExternalClosedOn);
		Assert.Equal(60.82m, actual.Bet);
		Assert.Equal(62.7m, actual.Validbet);
		Assert.Equal(transactionInfo.Winloss + transactionInfo.Validbet, actual.Settle);
		Assert.Equal(60.82m, actual.Winloss);
		Assert.Equal(1.88m, actual.Commission);
	}

	[Fact]
	public void Status_Is_Void()
	{
		var transactionInfo = new TransactionInfo(
			"87032f02ea3d11ed853f5254002237ea",
			"549-1683179782-585257-1-36",
			549,
			"4ucmqplxb029",
			"Void",
			DateTimeOffset.Parse("2023-05-04T13:36:16.185+08:00"),
			DateTimeOffset.Parse("2023-05-04T13:35:54.886+08:00"),
			DateTimeOffset.Parse("2023-05-04T13:36:16.092+08:00"),
			33.0m,
			0.0m,
			0.0m,
			0.0m,
			"{\"txId\":\"87032f02ea3d11ed853f5254002237ea\""
			);

		var utensil = new GameWagerUtensil();

		var actual = utensil.TransactionConvertGameWager(transactionInfo);

		Assert.NotNull(actual);
		Assert.Equal("549-1683179782-585257-1-36", actual.ExternalRoundId);
		Assert.Equal("87032f02ea3d11ed853f5254002237ea", actual.ExternalWagerId);
		Assert.Equal("549", actual.ExternalGameId);
		Assert.Equal("4ucmqplxb029", actual.PlayerCode);
		Assert.True(actual.IsClosed);
		Assert.True(actual.IsVoid);
		Assert.Equal(DateTimeOffset.Parse("2023-05-04T13:36:16.185+08:00"), actual.UpdateTime);
		Assert.Equal(DateTimeOffset.Parse("2023-05-04T13:35:54.886+08:00"), actual.ExternalCreatedOn);
		Assert.Equal(DateTimeOffset.Parse("2023-05-04T13:36:16.092+08:00"), actual.ExternalClosedOn);
		Assert.Equal(33.0m, actual.Bet);
		Assert.Equal(0.0m, actual.Validbet);
		Assert.Equal(0.0m, actual.Settle);
		Assert.Equal(0.0m, actual.Winloss);
		Assert.Equal(0.0m, actual.Commission);
	}
}
