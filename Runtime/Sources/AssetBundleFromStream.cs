// SPDX-License-Identifier: Apache-2.0
// © 2023-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Depra.Asset.Bundle.Extensions;
using Depra.Asset.Exceptions;
using Depra.Asset.ValueObjects;
using UnityEngine;

namespace Depra.Asset.Bundle.Sources
{
	public readonly struct AssetBundleFromStream : IAssetBundleSource
	{
		FileSize IAssetBundleSource.Size(AssetBundle of) => AssetBundleSize.Evaluate(of);

		AssetBundle IAssetBundleSource.Load(string by)
		{
			Guard.AgainstFileNotFound(by);

			using var fileStream = new FileStream(by, FileMode.Open, FileAccess.Read);

			return AssetBundle.LoadFromStream(fileStream);
		}

		async Task<AssetBundle> IAssetBundleSource.LoadAsync(string by, Action<float> onProgress,
			CancellationToken cancellationToken)
		{
			Guard.AgainstFileNotFound(by);

			await using var stream = new FileStream(by, FileMode.Open, FileAccess.Read);

			return await AssetBundle
				.LoadFromStreamAsync(stream)
				.ToTask(onProgress, cancellationToken);
		}
	}
}