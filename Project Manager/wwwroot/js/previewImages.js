const beforeImageInput = document.getElementById("before-image-file");
const afterImageInput = document.getElementById("after-image-file");
const sketchImageInput = document.getElementById("sketch-image-file");
const patternInput = document.getElementById("pattern-file");

const changeImageFile = (element) => {

    let elementParent = element.target.parentElement;
    let elementSiblingImg = elementParent.children[1];    

    if (elementSiblingImg.hasAttribute("hidden")) {
        elementSiblingImg.removeAttribute("hidden");
        elementSiblingImg.src = URL.createObjectURL(element.target.files[0]);
    } else if (!elementSiblingImg.hasAttribute("hidden")) {
        elementSiblingImg.setAttribute("hidden", true);
        elementSiblingImg.src = "";
    }
}

beforeImageInput.addEventListener("change", changeImageFile);
afterImageInput.addEventListener("change", changeImageFile);
sketchImageInput.addEventListener("change", changeImageFile);