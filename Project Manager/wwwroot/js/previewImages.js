const beforeImageInput = document.getElementById("BeforeImageFile");
const afterImageInput = document.getElementById("AfterImageFile");
const sketchImageInput = document.getElementById("SketchImageFile");
const patternInput = document.getElementById("PatternFile");

const changeImageFile = (element) => {

    let elementInputImageSource = element.target.value;
    let elementInputClasslist = element.target.classList;
    let elementParent = element.target.parentElement;
    let elementSiblingImg = elementParent.children[1];

    if (elementInputClasslist.contains("image-preview")) {

        if (elementInputImageSource == "") {
            elementSiblingImg.src = "/lib/icons/noImage.svg";
        } else{
            elementSiblingImg.src = URL.createObjectURL(element.target.files[0]);
        }
    }
}

beforeImageInput.addEventListener("change", changeImageFile);
afterImageInput.addEventListener("change", changeImageFile);
sketchImageInput.addEventListener("change", changeImageFile);