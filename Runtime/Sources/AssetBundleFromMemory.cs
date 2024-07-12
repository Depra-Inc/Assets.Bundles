// SPDX-License-Identifier: Apache-2.0
// © 2023-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Depra.Assets.Bundle.Extensions;
using Depra.Assets.ValueObjects;
using Depra.Assets.Exceptions;
using UnityEngine;

namespace Depra.Assets.Bundle.Sources
{
	public readonly struct AssetBundleFromMemory : IAssetBundleSource
	{
		FileSize IAssetBundleSource.Size(AssetBundle of) => AssetBundleSize.Evaluate(of);

		AssetBundle IAssetBundleSource.Load(string by)
		{
			Guard.AgainstFileNotFound(by);

			return AssetBundle.LoadFromMemory(File.ReadAllBytes(by));
		}

		Task<AssetBundle> IAssetBundleSource.LoadAsync(string by, Action<float> onProgress, CancellationToken cancellationToken)
		{
			Guard.AgainstFileNotFound(by);

			return AssetBundle
				.LoadFromMemoryAsync(File.ReadAllBytes(by))
				.ToTask(onProgress, cancellationToken);
		}
	}
}