console.clear();

// changing JavaScript objects is fast, changing the DOM is slow
// do as much as we can in JavaScript, then update the DOM at the end
(function() {
  var list = [];
  var filteredList = [];
  var maxDisplayLimit = 10;
  var textInput = document.querySelector('.text-filter');
  var displayList = document.querySelector('.list');
  var countMessage = document.querySelector('.count-message');
  
  function generateDummyList(itemCount) {
    if (!itemCount) {
      return;
    }
    for (var i = 0; i < itemCount; i++) {
      var item = {
        name: Math.random().toString(36).substr(2, 10),
        type: Math.random().toString(36).substr(2, 10),
        category: Math.random().toString(36).substr(2, 10)
      }
      list.push(item);
    }
  }
  
  function generateCountMessage() {
    var message = '';
    var matches = filteredList.length;
    switch(true) {
      case (matches === 0): 
        message = 'No matches found';
        break;
      case (matches === 1):
        message = 'Showing 1 item';
        break;
      case (matches <= maxDisplayLimit):
        message = 'Showing ' + filteredList.length + ' items';
        break;
      default:
        message = 'Showing ' + maxDisplayLimit + ' of ' + matches + ' items';
    }
    countMessage.textContent = message;
  }
  
  function generateListItem(item) {
    var li = document.createElement('li');
    var spanName = document.createElement('span');
    var spanType = document.createElement('span');
    var spanCategory = document.createElement('span');
    
    spanName.classList.add('name');
    spanType.classList.add('type');
    spanCategory.classList.add('category');
    
    spanName.textContent = item.name;
    spanType.textContent = item.type;
    spanCategory.textContent = item.category;
    
    li.appendChild(spanName);
    li.appendChild(spanType);
    li.appendChild(spanCategory);
    
    return li;
  }
  
  // convert the data array into an HTML list
  function generateList() {
    // document fragment is in memory and not part of the main DOM tree
    // appending children to it does not cause page reflow (computation of element's position and geometry)
    var frag = document.createDocumentFragment();
    for (var i = 0; i < filteredList.length; i++) {
      // set a limit on how many items we will show
      if (i < maxDisplayLimit) {
        var item = filteredList[i];
        var li = generateListItem(item);
        frag.appendChild(li);
      } else {
        break;
      }
    }
    displayList.innerHTML = '';
    // nothing touching the DOM until we have a complete Document Fragment
    // with all the list items ready to append to our list
    displayList.appendChild(frag);
    generateCountMessage();
  }
  
  function textMatch(item) {
    var searchTerm = textInput.value.toLowerCase();
    var itemText = (item.name + item.type + item.category).toLowerCase();
    return itemText.indexOf(searchTerm) !== -1;
  }
  
  function getFilteredItems() {
    // get the data list with the full data
    // filter it down into a smaller array using textMatch
    filteredList = list.filter(textMatch);
    generateList();
  }
  
  textInput.addEventListener('keyup', getFilteredItems);
  
  //generateDummyList(100000);
  getFilteredItems();
  
})();