namespace SceneDataViewer
{
    public interface IDataPanel
    {
        /// <summary>
        /// 初期化します。
        /// </summary>
        void Initialize();

        /// <summary>
        /// データを更新します。
        /// </summary>
        void Update();

        /// <summary>
        /// GUI を描画します。
        /// </summary>
        void OnGUI();
    }
}