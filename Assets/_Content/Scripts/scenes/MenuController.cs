using System;
using _Content.Scripts.so;
using Melanchall.DryWetMidi.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Content.Scripts.scenes
{
    public class MenuController : MonoBehaviour
    {

        public static string chosenTrackId;
        public static string chosenMidiFile;
        public static bool menuOverride = false;

        private Button startButton;

        public TMP_Dropdown dropdownTracks;
        public TMP_InputField inputFieldMidi;
        public TMP_Text warningText;
        private void OnEnable()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;

            startButton = root.Q<Button>("ButtonStart");
            startButton.clicked += OnStartClicked;

            dropdownTracks.options.Clear();
            foreach (var track in AllTracks.Instance.tracks)
            {
                dropdownTracks.options.Add (new TMP_Dropdown.OptionData() {text=track.id});
            }
            
            dropdownTracks.onValueChanged.AddListener(delegate {
                inputFieldMidi.text = dropdownTracks.options[dropdownTracks.value].text + ".mid";
            });
            
            inputFieldMidi.onValueChanged.AddListener(delegate {
                try
                {
                    var midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + inputFieldMidi.text);
                    warningText.gameObject.SetActive(false);
                }
                catch (Exception e)
                {
                    warningText.gameObject.SetActive(true);
                    return;
                }
            });

            inputFieldMidi.text = dropdownTracks.options[dropdownTracks.value].text + ".mid";
            
            warningText.gameObject.SetActive(false);
            
        }

        private void OnDisable()
        {
            startButton.clicked -= OnStartClicked;
        }

        private void OnStartClicked()
        {
            try
            {
                var midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + inputFieldMidi.text);
            }
            catch (Exception e)
            {
                warningText.gameObject.SetActive(true);
                return;
            }
            
            
            chosenTrackId = dropdownTracks.options[dropdownTracks.value].text;
            chosenMidiFile = inputFieldMidi.text;
            menuOverride = true;
            
            SceneController.Instance.Load("zuma_from_scratch_2");
        }
    }
}
