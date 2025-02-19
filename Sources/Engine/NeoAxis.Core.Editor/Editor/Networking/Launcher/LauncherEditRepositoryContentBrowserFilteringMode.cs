﻿#if CLOUD
#if !DEPLOY
// Copyright (C) NeoAxis Group Ltd. 8 Copthall, Roseau Valley, 00152 Commonwealth of Dominica.
using System;
using System.Collections.Generic;

namespace NeoAxis.Editor
{
	public class LauncherEditRepositoryContentBrowserFilteringMode : ContentBrowserFilteringMode
	{
		public LauncherEditRepositoryContentBrowserFilteringMode()
		{
		}

		public override string Name
		{
			get { return "LauncherEditRepository"; }
		}

		//public override bool AddGroupGeneral
		//{
		//	get { return false; }
		//}

		public override bool AddGroupsBaseTypesAddonsProject
		{
			get { return false; }
		}

		public override bool AddGroupFavorites
		{
			get { return false; }
		}

		//public override bool AddSolution
		//{
		//	get { return true; }
		//}

		public override bool AddGroupAllTypes
		{
			get { return false; }
		}

		public override bool AddFiles
		{
			get { return false; }
		}

		//public override bool AddAllFiles
		//{
		//	get { return true; }
		//}

		//public override string[] FileSearchPatterns
		//{
		//	get { return new string[] { "*.cs" }; }
		//}

		public override bool HideDirectoriesWithoutItems
		{
			get { return true; }
		}

		//public override bool ExpandAllFileItemsAtStartup
		//{
		//	get { return true; }
		//}
	}
}
#endif
#endif