// SPDX-License-Identifier: Apache-2.0
// © 2023-2025 Nikolay Melnikov <n.melnikov@depra.org>

using System;

namespace Depra.Assets.Bundle
{
	internal sealed class AssetBundleFileLoadingFailed : Exception
	{
		public AssetBundleFileLoadingFailed(string name, string bundleName) : base(
			$"File with name '{name}' form asset bundle '{bundleName}' was not loaded!") { }
	}
}