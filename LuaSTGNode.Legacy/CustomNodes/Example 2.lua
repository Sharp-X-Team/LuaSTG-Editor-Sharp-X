function InitNode()
	local properties = {
		name = "Example Node 2",
		image = "repeat.png",

		Parameters = {
			{"Color", "COLOR_RED", "color"}
		}
	}
	
	return properties
end

function ToLuaHead()
	return "-- Custom node head code"
end

function ToLuaBody()
	local lua_code = [[-- Put the lua code here.
self.color = {0}
]]
	return lua_code
end

function ToLuaTail()
	return "-- Custom node tail code"
end

function ToString()
	local node_description = "Self's color is {0}"
	return node_description
end