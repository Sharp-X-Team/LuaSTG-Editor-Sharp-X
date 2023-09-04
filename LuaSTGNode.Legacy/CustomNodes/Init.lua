function InitNodes()
	-- Put the name of node's file with or without the .lua at the end of it.
	local Registered_Nodes = {
		"Example",
		"_Separator", -- See documentation. You can't name a node "_Separator", it's a reserved name.
		"Example 2",
		"Example 3"
	}

	-- Returing the Registered_Nodes array for the Editor to know what nodes to load at startup.
	return Registered_Nodes
end