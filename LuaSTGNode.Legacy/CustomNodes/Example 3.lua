function InitNode()
	local properties = {
		name = "Example Node 3",

		leafNode = true, -- Cannot have any children. true/false.
		RequireParent = "CodeAlike", -- See documentation
	}
	
	return properties
end