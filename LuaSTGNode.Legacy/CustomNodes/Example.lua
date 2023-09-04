function InitNode()
	local properties = {
		name = "Example Node",

		leafNode = true, -- Cannot have any children. true/false.
		RequireParent = "CodeAlike", -- See documentation
		Parameters = {
			{"Position", "self.x,self.y", "position"},
			{"Bound", "true", "bool"}
		}
	}
	
	return properties
end

function ToString()
	return {"Does something at position (%s) with bound set to \"%s\"", "Position", "Bound"}
end