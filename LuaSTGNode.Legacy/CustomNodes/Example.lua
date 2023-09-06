function InitNode()
	local properties = {
		name = "Example Node",

		leafNode = true, -- Cannot have any children. true/false.
		RequireParent = "CodeAlike", -- See documentation
		Parameters = {
			{"Unit", "self", "target"},
			{"Position", "0,0", "position"}
		}
	}
	
	return properties
end

function ToLua()
	return {"%s.x, %s.y = %s", "Unit", "Unit", "Position"}
end

function ToString()
	return {"Sets {0}'s position to ({1})", "Unit", "Position"}
end