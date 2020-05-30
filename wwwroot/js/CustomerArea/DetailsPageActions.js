function AddEventListenersToChangeProductFeaturedImage() {
    let smallImages = document.getElementsByClassName('img-small-wrap')[0];
    const bigImage = document.getElementsByClassName('img-big-wrap')[0];

    if (smallImages == null)
        return;

    smallImages = smallImages.getElementsByTagName('img');

    for (let index = 0; index < smallImages.length; index++) {
        smallImages[index].addEventListener('click', () => {
            const imagePath = smallImages[index].getAttribute('src');
            bigImage.getElementsByTagName('a')[0].setAttribute('href', imagePath);
            bigImage.getElementsByTagName('img')[0].setAttribute('src', imagePath);
        });
    }
}

AddEventListenersToChangeProductFeaturedImage();