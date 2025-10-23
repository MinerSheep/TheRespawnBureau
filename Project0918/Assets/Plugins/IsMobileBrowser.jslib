mergeInto(LibraryManager.library, {
  IsMobileBrowser: function () {
    try {
      var ua = navigator.userAgent || "";
      // Mobile == 1
      return +(/Android|iPhone|iPad|iPod/i.test(ua));
    } catch (e) { return 0; }
  }
});
