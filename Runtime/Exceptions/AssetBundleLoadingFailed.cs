// SPDX-License-Identifier: Apache-2.0
// © 2023-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;

namespace Depra.Asset.Bundle.Exceptions
{
	internal sealed class AssetBundleLoadingFailed : Exception
	{
		public AssetBundleLoadingFailed(string path) : base($"Asset bundle at path '{path}' was not loaded!") { }
	}
}