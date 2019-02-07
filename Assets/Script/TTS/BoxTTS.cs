using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTTS : MonoBehaviour
{
    public BoxTTS nextVertical;
    public BoxTTS prevVertical;

    public BoxTTS nextHorizontal;
    public BoxTTS prevHorizontal;

    public Color colorSelected;
    public Color colorFocus;
    public Color colorAnswered;

    [SerializeField] SpriteRenderer backCol;
    [SerializeField] TextMesh text;

    public Soal soalVertical;
    public Soal soalHorizontal;


    bool _wasAnswer;
    public bool wasAnswer { get { return _wasAnswer; }
        set {
            _wasAnswer = value;
            if (wasAnswer) {
                backCol.color = colorAnswered;
            }
        }
    }

    bool _isFocus;
    public bool isFocus {
        get { return _isFocus; }
        set {
            _isFocus = value;
            if (_isFocus) {
                backCol.color = colorFocus;
                this.transform.SetAsFirstSibling();
            }
            else
            {
                backCol.color = colorSelected;
            }
        }
    }
    public string boxAnswer;
    string _currentAnswer = "";
    public string currentAnswer{
        get { return _currentAnswer; }
        set { _currentAnswer = value;
            text.text = _currentAnswer;
            backCol.color = colorSelected;
        }
    }

    public void setToFocus()
    {
        if (wasAnswer)
        {
            backCol.color = colorAnswered;
        }
        else
        {
            backCol.color = colorFocus;
            Vector3 p = backCol.transform.position;
            p.z = 0.8f;
            backCol.transform.position = p;

        }
        
        
    }

    public void setToSelected()
    {
        if (wasAnswer) {
            backCol.color = colorAnswered;
        }
        else
        {
            backCol.color = colorSelected;
            Vector3 p = backCol.transform.position;
            p.z = 1f;
            backCol.transform.position = p;
        }
        
    }

    public bool CheckAnswerAllBox(Arah arah ) {

        bool isTrue = false;
        List<GameObject> box = new List<GameObject>();
        box.Add(this.gameObject);
        switch (arah) {
            case Arah.Vertical:
                isTrue = (nextVertical !=null ? nextVertical.CheckAnswerAllBox(arah, ref box, true):true) &&  ( prevVertical != null ? prevVertical.CheckAnswerAllBox(arah, ref box, false):true) && _currentAnswer == boxAnswer;
                break;
            case Arah.Horizontal:
                isTrue = (nextHorizontal != null ? nextHorizontal.CheckAnswerAllBox(arah, ref box, true) : true) && (prevHorizontal != null ? prevHorizontal.CheckAnswerAllBox(arah, ref box, false) : true) && _currentAnswer == boxAnswer;
                break;

        }
        Debug.Log("Answer is : " + isTrue);
        if (isTrue) {
            foreach (GameObject b in box) {
                b.GetComponent<BoxTTS>().wasAnswer = true;

            }
        }
        return isTrue;
    }

    public bool CheckAnswerAllBox(Arah arah, ref List<GameObject> box, bool toNext)
    {
        bool isTrue = false;
        box.Add(this.gameObject);
        switch (arah)
        {
            case Arah.Vertical:
                if (toNext)
                {
                    isTrue = (nextVertical != null ? nextVertical.CheckAnswerAllBox(arah, ref box, true) : true) && _currentAnswer == boxAnswer;
                }
                else {
                    isTrue = (prevVertical != null ? prevVertical.CheckAnswerAllBox(arah, ref box, false) : true) && _currentAnswer == boxAnswer;
                }

                break;
            case Arah.Horizontal:
                if (toNext)
                {
                    isTrue = (nextHorizontal != null ? nextHorizontal.CheckAnswerAllBox(arah, ref box, true) : true) && _currentAnswer == boxAnswer;
                }
                else
                {
                    isTrue = (prevHorizontal != null ? prevHorizontal.CheckAnswerAllBox(arah, ref box, false) : true) && _currentAnswer == boxAnswer;
                }
                break;
        }

        return isTrue;
    }

    public BoxTTS ClearAll(Arah arah) {
        if (wasAnswer)
        {
            
            Debug.Log("Tidak Dapat Dihapus");
            setToSelected();

        }
        else {
            currentAnswer = "";
        }

        if (arah == Arah.Vertical) {
            if (nextVertical != null) {
                nextVertical.ClearAll(arah , true);
            }
            if (prevVertical != null)
            {
                return prevVertical.ClearAll(arah , false);
            }
            else {
                return this;
            }
        }
        else
        {
            if (nextHorizontal != null)
            {
                nextHorizontal.ClearAll(arah,true);
            }
            if (prevHorizontal != null)
            {
                return prevHorizontal.ClearAll(arah , false);
            }
            else {
                return this;
            }

        }

    }

    public BoxTTS ClearAll(Arah arah ,  bool toNext) {
        if (wasAnswer)
        {

            Debug.Log("Tidak Dapat Dihapus");
            setToSelected();

        }
        else
        {
            currentAnswer = "";
        }

        if (arah == Arah.Vertical)
        {
            if (nextVertical != null && toNext)
            {
                nextVertical.ClearAll(arah, true);
            }
            if (prevVertical != null && !toNext)
            {
                return prevVertical.ClearAll(arah, false);
            }
            else if (!toNext)
            {
                return this;
            }
        }
        else
        {
            if (nextHorizontal != null && toNext)
            {
                nextHorizontal.ClearAll(arah, true);
            }
            if (prevHorizontal != null && !toNext)
            {
                return prevHorizontal.ClearAll(arah, false);
            }
            else if(!toNext)
            {
                return this;
            }

        }
        return null;
    }

    public BoxTTS getNextBox(Arah arah) {
        if (arah == Arah.Horizontal)
        {
            
            if (nextHorizontal != null && nextHorizontal.currentAnswer != "") {
                return nextHorizontal.getNextBox(arah);
            }

            return (nextHorizontal!=null?nextHorizontal:this);
        }
        else {

            if (nextVertical != null && nextVertical.currentAnswer != "")
            {
                return nextVertical.getNextBox(arah);
            }
            return (nextVertical != null ? nextVertical : this);
        }
    }
    public BoxTTS getPrevBox(Arah arah)
    {
        if (arah == Arah.Horizontal)
        {
            return (prevHorizontal != null ? prevHorizontal : this); 
        }
        else
        {
            return (prevVertical != null ? prevVertical : this);
        }
    }

    public void activateSelected( ref List<GameObject> allBoxObj , ref Arah currArah , ref BoxTTS prior) {

        // Jika dia adalah persimpangan
        if ((nextHorizontal != null || prevHorizontal != null) && (nextVertical != null || prevVertical != null)) {
            this.gameObject.name = "persimpangan";
            Debug.Log("Persimpangan");
            return;
        }

        allBoxObj.Add(this.gameObject);
        backCol.gameObject.SetActive(true);
        setToSelected();

        if (_currentAnswer == "") {
            prior = this;
        }
        

        if (nextHorizontal != null || prevHorizontal != null)
        {
            currArah = Arah.Horizontal;
            if (nextHorizontal != null && _currentAnswer != "")
            {
                nextHorizontal.activateSelected(Arah.Horizontal, true, ref allBoxObj, ref prior);
            }
            else if (nextHorizontal != null)
            {
                nextHorizontal.activateSelected(Arah.Horizontal, true, ref allBoxObj);
            }
            else { 
                prior = this;
            }


            if (prevHorizontal != null && _currentAnswer == "") {
                prevHorizontal.activateSelected(Arah.Horizontal , false , ref allBoxObj, ref prior);
            }else if (prevHorizontal != null)
            {
                prevHorizontal.activateSelected(Arah.Horizontal, false, ref allBoxObj);
            }
        }
        else if (nextVertical != null || prevVertical != null) {
            currArah = Arah.Vertical;
            if (nextVertical != null && _currentAnswer != "")
            {
                nextVertical.activateSelected(Arah.Vertical, true, ref allBoxObj, ref prior);
            }
            else if (nextVertical != null)
            {
                nextVertical.activateSelected(Arah.Vertical, true, ref allBoxObj);
            }
            else
            {
                prior = this;
            }



            if (prevVertical != null && _currentAnswer == "")
            {
                prevVertical.activateSelected(Arah.Vertical,false , ref allBoxObj, ref prior);
            }else if (prevVertical != null)
            {
                prevVertical.activateSelected(Arah.Vertical, false, ref allBoxObj);
            }
        }


    }

    public void activateSelected(ref List<GameObject> allBoxObj, ref Arah currArah, ref BoxTTS prior , Arah arah)
    {

        allBoxObj.Add(this.gameObject);
        backCol.gameObject.SetActive(true);
        setToSelected();

        if (_currentAnswer == "")
        {
            prior = this;
        }


        if (arah == Arah.Horizontal)
        {
            currArah = Arah.Horizontal;
            if (nextHorizontal != null && _currentAnswer != "")
            {
                nextHorizontal.activateSelected(Arah.Horizontal, true, ref allBoxObj, ref prior);
            }
            else if (nextHorizontal != null)
            {
                nextHorizontal.activateSelected(Arah.Horizontal, true, ref allBoxObj);
            }
            else
            {
                prior = this;
            }


            if (prevHorizontal != null && _currentAnswer == "")
            {
                prevHorizontal.activateSelected(Arah.Horizontal, false, ref allBoxObj, ref prior);
            }
            else if (prevHorizontal != null)
            {
                prevHorizontal.activateSelected(Arah.Horizontal, false, ref allBoxObj);
            }
        }
        else if (arah == Arah.Vertical)
        {
            currArah = Arah.Vertical;
            if (nextVertical != null && _currentAnswer != "")
            {
                nextVertical.activateSelected(Arah.Vertical, true, ref allBoxObj, ref prior);
            }
            else if (nextVertical != null)
            {
                nextVertical.activateSelected(Arah.Vertical, true, ref allBoxObj);
            }
            else
            {
                prior = this;
            }



            if (prevVertical != null && _currentAnswer == "")
            {
                prevVertical.activateSelected(Arah.Vertical, false, ref allBoxObj, ref prior);
            }
            else if (prevVertical != null)
            {
                prevVertical.activateSelected(Arah.Vertical, false, ref allBoxObj);
            }
        }


    }

    public void activateSelected( Arah arah, bool toNext, ref List<GameObject> allBoxObj)
    {
        allBoxObj.Add(this.gameObject);
        backCol.gameObject.SetActive(true);
        setToSelected();

        switch (arah) {
            case Arah.Horizontal:
                if (toNext)
                {
                    if (nextHorizontal != null)
                        nextHorizontal.activateSelected(Arah.Horizontal , true, ref allBoxObj);
                }
                else {
                    if (prevHorizontal != null)
                        prevHorizontal.activateSelected(Arah.Horizontal, false ,ref allBoxObj);
                }
                    

                break;
            case Arah.Vertical:
                if (toNext)
                {
                    if (nextVertical != null)
                        nextVertical.activateSelected(Arah.Vertical, true, ref allBoxObj);
                }
                else
                {
                    if (prevVertical != null)
                        prevVertical.activateSelected(Arah.Vertical, false, ref allBoxObj);
                }
                break;

        }

       
    }
    public void activateSelected(Arah arah, bool toNext ,  ref List<GameObject> allBoxObj, ref BoxTTS prior)
    {
        allBoxObj.Add(this.gameObject);
        backCol.gameObject.SetActive(true);

        if (_currentAnswer == "")
        {
            prior = this;
        }

        switch (arah)
        {
            case Arah.Horizontal:

                if (toNext)
                {
                    if (nextHorizontal != null)
                    {
                        nextHorizontal.activateSelected(Arah.Horizontal, true, ref allBoxObj, ref prior);
                    }
                    else {
                        prior = this;
                    }
                }
                else
                {
                    if (prevHorizontal != null)
                        prevHorizontal.activateSelected(Arah.Horizontal, false, ref allBoxObj, ref prior);
                }
                break;
            case Arah.Vertical:

                if (toNext)
                {
                    if (nextVertical != null)
                    {
                        nextVertical.activateSelected(Arah.Vertical, true, ref allBoxObj, ref prior);
                    }
                    else
                    {
                        prior = this;
                    }
                }
                else
                {
                    if (prevVertical != null)
                        prevVertical.activateSelected(Arah.Vertical, false, ref allBoxObj, ref prior);
                }
                break;

        }
    }


}
