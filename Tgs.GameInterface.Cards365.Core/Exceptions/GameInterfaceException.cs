namespace Tgs.GameInterface.Cards365.Core.Exceptions;

public class GameInterfaceException(string status) : Exception($"{GetMessage(status)}({status})")
{
	private static string GetMessage(string status)
		=> status switch
		{
			"0" => "Fail (returned by Game Provider) 失败（由游戏提供商返回）",
			"1001" => "Not Authorized.未经授权。",
			"1002" => "Balance is invalid.无效余额。",
			"1003" => "No Data 没有数据",
			"1004" => "Username is invalid.无效的用户名。",
			"1006" => "Insufficient parameters.参数不足。",
			"1007" => "Not supported API.不支持 API。",
			"1008" => "Invalid parameters. 参数无效。",
			"1009" => "Insufficient balance. 余额不足。",
			"1011" => "Game ID not exist. 游戏代码不存在。",
			"1012" => "Agent does not exist. 代理不存在。",
			"1014" => "Query too fast.查询太快了。",
			"1015" => "Invalid URL. 无效的网址。",
			"1016" => "Invalid Agent. 无效的代理。",
			"1018" => "Account locked. 帐户被锁定。",
			"1023" => "Invalid website. 网站无效。",
			"1024" => "Invalid currency setting.货币设置无效。",
			"1030" => "Agent is not belong to website 不是运营商可使用的代理",
			"1031" => "ts_code already exists.ts_code 已存在",
			"1032" => "User does not exist.用户不存在。",
			_ => status
		};
}
