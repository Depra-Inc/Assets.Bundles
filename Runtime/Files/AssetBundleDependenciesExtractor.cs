// SPDX-License-Identifier: Apache-2.0
// © 2023-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Depra.Assets.ValueObjects;
using Depra.Assets.Bundle.Exceptions;
using UnityEngine;

namespace Depra.Assets.Bundle
{
	internal static class AssetBundleDependenciesExtractor
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IEnumerable<IAssetUri> Extract(AssetBundle assetBundle)
		{
			const string MANIFEST_NAME = "assetbundlemanifest";
			var manifest = assetBundle.LoadAsset<AssetBundleManifest>(MANIFEST_NAME);
			if (manifest == null)
			{
				throw new AssetBundleFileLoadingFailed(MANIFEST_NAME, assetBundle.name);
			}

			var dependencies = manifest.GetAllDependencies(assetBundle.name);
			foreach (var dependencyPath in dependencies)
			{
				yield return new AssetBundleUri(dependencyPath);
			}
		}
	}
}