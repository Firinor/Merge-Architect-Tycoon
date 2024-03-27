using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CreateBuildingPopup : MonoBehaviour
{
    public Button goToMergePanelButton;
    public Button createBuildingButton;

    private MenuButtonsWidgetController _sceneButtons;
    private CreateBuildingPopupPresenter _createBuildingPopupPresenter;

    [Inject]
    void Construct(CreateBuildingPopupPresenter createBuildingPopupPresenter,
        MenuButtonsWidgetController _sceneButtons)
    {
        this._sceneButtons = _sceneButtons;
        _createBuildingPopupPresenter = createBuildingPopupPresenter;
    }

    public void InitializePopup()
    {
        goToMergePanelButton.onClick.AddListener(GoToMergePanel);
        createBuildingButton.onClick.AddListener(_createBuildingPopupPresenter.CreateBuildingButtonClicked);
    }

    public void OpenMergeButton()
    {
        goToMergePanelButton.gameObject.SetActive(true);
        createBuildingButton.gameObject.SetActive(false);
    }

    public void OpenCreateBuildingButton()
    {
        createBuildingButton.gameObject.SetActive(true);
        goToMergePanelButton.gameObject.SetActive(false);
    }

    public void HideButtons()
    {
        createBuildingButton.gameObject.SetActive(false);
        goToMergePanelButton.gameObject.SetActive(false);
    }

    private void GoToMergePanel()
    {
        _sceneButtons.OnMenuButtonClick(MenuButtonsEnum.Merge);
    }
}