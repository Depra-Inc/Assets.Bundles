// SPDX-License-Identifier: Apache-2.0
// © 2023-2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Depra.Assets.Delegates;
using Depra.Assets.Files;
using Depra.Assets.ValueObjects;
using Depra.Assets.Bundle.Exceptions;
using Depra.Assets.Bundle.Sources;
using Depra.Assets.Exceptions;
using UnityEngine;

namespace Depra.Assets.Bundle
{
	public sealed class AssetBundleFile : IAssetFile<AssetBundle>, IDisposable
	{
		private readonly AssetBundleUri _uri;
		private readonly IAssetBundleSource _source;

		private AssetBundle _loadedAssetBundle;

		public AssetBundleFile(AssetBundleUri uri, IAssetBundleSource source)
		{
			Guard.AgainstNull(uri, nameof(uri));
			Guard.AgainstNull(source, nameof(source));

			_source = source;
			Metadata = new AssetMetadata(_uri = uri, FileSize.Unknown);
		}

		public AssetMetadata Metadata { get; }
		public bool IsLoaded => _loadedAssetBundle != null;

		public AssetBundle Load()
		{
			if (IsLoaded)
			{
				return _loadedAssetBundle;
			}

			var loadedAssetBundle = _source.Load(by: _uri.AbsolutePathWithoutExtension);
			Guard.AgainstNull(loadedAssetBundle, () => new AssetBundleLoadingFailed(_uri.Absolute));

			_loadedAssetBundle = loadedAssetBundle;
			Metadata.Size = _source.Size(of: _loadedAssetBundle);

			return _loadedAssetBundle;
		}

		public async Task<AssetBundle> LoadAsync(DownloadProgressDelegate onProgress = null,
			CancellationToken cancellationToken = default)
		{
			if (IsLoaded)
			{
				onProgress?.Invoke(DownloadProgress.Full);
				return _loadedAssetBundle;
			}

			var loadedAssetBundle = await _source
				.LoadAsync(_uri.AbsolutePathWithoutExtension, OnProgress, cancellationToken);

			Guard.AgainstNull(loadedAssetBundle, () => new AssetBundleLoadingFailed(_uri.Absolute));

			_loadedAssetBundle = loadedAssetBundle;
			onProgress?.Invoke(DownloadProgress.Full);
			Metadata.Size = _source.Size(of: _loadedAssetBundle);

			return _loadedAssetBundle;

			void OnProgress(float progress) => onProgress?.Invoke(new DownloadProgress(progress));
		}

		public void Unload()
		{
			if (IsLoaded == false)
			{
				return;
			}

			_loadedAssetBundle.Unload(true);
			_loadedAssetBundle = null;
		}

		public void UnloadAsync()
		{
			if (IsLoaded == false)
			{
				return;
			}

			_loadedAssetBundle.UnloadAsync(true);
			_loadedAssetBundle = null;
		}

		public IEnumerable<IAssetUri> Dependencies()
		{
			Guard.Against(IsLoaded == false, () => new AssetBundleLoadingFailed(_uri.Absolute));
			return AssetBundleDependenciesExtractor.Extract(_loadedAssetBundle);
		}

		void IDisposable.Dispose() => Unload();
	}
}