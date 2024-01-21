function InitNode()
	local properties = {
		name = "Example Node",
		image = "if.png",

		Parameters = {
			{"Unit", "self", "target"},
			{"Position", "0,0", "position"}
		}
	}
	
	return properties
end

function ToLuaHead()
	return "-- Custom node head code"
end

function ToLuaBody()
	local lua_code = [[-- Put the lua code here.
{0}.x, {0}.y = {1}
	-- test indent.
]]
	return lua_code
end

function ToLuaTail()
	return "-- Custom node tail code"
end

function ToString()
	local node_description = "Sets {0}'s position to ({1})"
	return node_description
end