// SPDX-License-Identifier: Apache-2.0
// © 2023-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;

namespace Depra.Assets.Bundle.Exceptions
{
	internal sealed class AssetBundleFileLoadingFailed : Exception
	{
		public AssetBundleFileLoadingFailed(string name, string bundleName) : base(
			$"File with name '{name}' form asset bundle '{bundleName}' was not loaded!") { }
	}
}