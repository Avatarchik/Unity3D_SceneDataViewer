using UnityEngine;
using XJGUI = XJ.Unity3D.GUI;

namespace SceneDataViewer
{
    [System.Serializable]
    public class ObjectDataPanel : IDataPanel
    {
        #region Field

        // Object に関連するデータを更新する時間を設定します。
        // 0.5 を指定するとき、0.5 sec おきに更新します。
        // 設定値が 0 より大きいときだけ関連するデータが表示されます。

        public float dataUpdateTimeInSec = 0;

        private float elapsedTimeToUpdateDataInSec;

        #region Data

        private UnityEngine.Object[] currentAllObjects;
        private UnityEngine.Object[] currentTextures;
        private UnityEngine.Object[] currentAudioClips;
        private UnityEngine.Object[] currentMeshes;
        private UnityEngine.Object[] currentMaterials;
        private UnityEngine.Object[] currentGameObjects;
        private UnityEngine.Object[] currentComponents;

        private int currentAllObjectCount;
        private int currentTextureCount;
        private int currentAudioClipCount;
        private int currentMeshCount;
        private int currentMaterialCount;
        private int currentGameObjectCount;
        private int currentComponentCount;

        private int maxAllObjectCount;
        private int maxTextureCount;
        private int maxAudioClipCount;
        private int maxMeshCount;
        private int maxMaterialCount;
        private int maxGameObjectCount;
        private int maxComponentCount;

        #endregion Data

        #region GUI

        private XJGUI.FoldoutPanel foldoutPanelAllObject;
        private XJGUI.FoldoutPanel foldoutPanelTexture;
        private XJGUI.FoldoutPanel foldoutPanelAudioClip;
        private XJGUI.FoldoutPanel foldoutPanelMesh;
        private XJGUI.FoldoutPanel foldoutPanelMaterial;
        private XJGUI.FoldoutPanel foldoutPanelGameObject;
        private XJGUI.FoldoutPanel foldoutPanelComponent;

        #endregion GUI

        #endregion Field

        #region Constructor

        public ObjectDataPanel()
        {
            Initialize();
        }

        #endregion Constructor

        #region Implement IDataPanel

        public void Initialize()
        {
            InitializeGUI();
        }

        public void Update()
        {
            if (this.dataUpdateTimeInSec <= 0)
            {
                return;
            }

            if (this.elapsedTimeToUpdateDataInSec < this.dataUpdateTimeInSec)
            {
                this.elapsedTimeToUpdateDataInSec += Time.deltaTime;

                return;
            }

            this.elapsedTimeToUpdateDataInSec = 0;

            UpdateSceneObjectsReferences();
            UpdateCurrentObjectCount();
            UpdateMaxObjectCount();
        }

        public void OnGUI()
        {
            if (this.dataUpdateTimeInSec <= 0)
            {
                return;
            }

            this.foldoutPanelAllObject.Controller("All : " + this.currentAllObjectCount, () =>
            {
                GUILayout.Label("― Max : " + this.maxAllObjectCount);
            });

            this.foldoutPanelTexture.Controller("Texture : " + this.currentTextureCount, () =>
            {
                GUILayout.Label("― Max : " + this.maxTextureCount);
            });

            this.foldoutPanelAudioClip.Controller("AudioClip : " + this.currentAudioClipCount, () =>
            {
                GUILayout.Label("― Max : " + this.maxAudioClipCount);
            });

            this.foldoutPanelMesh.Controller("Mesh : " + this.currentMeshCount, () =>
            {
                GUILayout.Label("― Max : " + this.maxMeshCount);
            });

            this.foldoutPanelMaterial.Controller("Material : " + this.currentMaterialCount, () =>
            {
                GUILayout.Label("― Max : " + this.maxMaterialCount);
            });

            this.foldoutPanelGameObject.Controller("GameObject : " + this.currentGameObjectCount, () =>
            {
                GUILayout.Label("― Max : " + this.maxGameObjectCount);
            });

            this.foldoutPanelComponent.Controller("Component : " + this.currentComponentCount, () =>
            {
                GUILayout.Label("― Max : " + this.maxComponentCount);
            });
        }

        #endregion Implement IDataPanel

        /// <summary>
        /// GUI を初期化します。
        /// </summary>
        private void InitializeGUI()
        {
            this.foldoutPanelAllObject = new XJGUI.FoldoutPanel(false);
            this.foldoutPanelTexture = new XJGUI.FoldoutPanel(false);
            this.foldoutPanelAudioClip = new XJGUI.FoldoutPanel(false);
            this.foldoutPanelMesh = new XJGUI.FoldoutPanel(false);
            this.foldoutPanelMaterial = new XJGUI.FoldoutPanel(false);
            this.foldoutPanelGameObject = new XJGUI.FoldoutPanel(false);
            this.foldoutPanelComponent = new XJGUI.FoldoutPanel(false);
        }

        /// <summary>
        /// 各オブジェクトの参照データを更新します。
        /// </summary>
        public void UpdateSceneObjectsReferences()
        {
            this.currentAllObjects = Resources.FindObjectsOfTypeAll(typeof(UnityEngine.Object));
            this.currentTextures = Resources.FindObjectsOfTypeAll(typeof(Texture));
            this.currentAudioClips = Resources.FindObjectsOfTypeAll(typeof(AudioClip));
            this.currentMeshes = Resources.FindObjectsOfTypeAll(typeof(Mesh));
            this.currentMaterials = Resources.FindObjectsOfTypeAll(typeof(Material));
            this.currentGameObjects = Resources.FindObjectsOfTypeAll(typeof(GameObject));
            this.currentComponents = Resources.FindObjectsOfTypeAll(typeof(Component));
        }

        /// <summary>
        /// 各オブジェクトの数データを更新します。
        /// </summary>
        public void UpdateCurrentObjectCount()
        {
            this.currentAllObjectCount = this.currentAllObjects.Length;
            this.currentTextureCount = this.currentTextures.Length;
            this.currentAudioClipCount = this.currentAudioClips.Length;
            this.currentMeshCount = this.currentMeshes.Length;
            this.currentMaterialCount = this.currentMaterials.Length;
            this.currentGameObjectCount = this.currentGameObjects.Length;
            this.currentComponentCount = this.currentComponents.Length;
        }

        /// <summary>
        /// 最大オブジェクト数データを更新します。
        /// </summary>
        private void UpdateMaxObjectCount()
        {
            if (this.currentAllObjectCount > this.maxAllObjectCount)
            {
                this.maxAllObjectCount = this.currentAllObjectCount;
            }
            if (this.currentTextureCount > this.maxTextureCount)
            {
                this.maxTextureCount = this.currentTextureCount;
            }
            if (this.currentAudioClipCount > this.maxAudioClipCount)
            {
                this.maxAudioClipCount = this.currentAudioClipCount;
            }
            if (this.currentMeshCount > this.maxMeshCount)
            {
                this.maxMeshCount = this.currentMeshCount;
            }
            if (this.currentMaterialCount > this.maxMaterialCount)
            {
                this.maxMaterialCount = this.currentMaterialCount;
            }
            if (this.currentGameObjectCount > this.maxGameObjectCount)
            {
                this.maxGameObjectCount = this.currentGameObjectCount;
            }
            if (this.currentComponentCount > this.maxComponentCount)
            {
                this.maxComponentCount = this.currentComponentCount;
            }
        }
    }
}