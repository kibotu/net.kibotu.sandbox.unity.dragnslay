using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.utility
{
    public class MapGenerator : MonoBehaviour
    {
        public RawImage level;

        public float ratio = 1f;
        public float radius = 2.5f;

        public void Awake()
        {
            float x0 = radius;
            float y0 = radius;
            float x1 = level.rectTransform.sizeDelta.x - radius;
            float y1 = level.rectTransform.sizeDelta.y - radius;
            float active = -1;
            float k = 100;
            Reset(2 * radius, Input.mousePosition.x * ratio, Input.mousePosition.y * ratio);
        }

        public void Reset(float radius, float x, float y)
        {
//            level = new RawImage();
        }

//  context.clearRect(0, 0, width, height);
//
//  var id = ++active,
//      inner2 = r * r,
//      A = 4 * r * r - inner2,
//      cellSize = r * Math.SQRT1_2,
//      gridWidth = Math.ceil(width / cellSize),
//      gridHeight = Math.ceil(height / cellSize),
//      grid = new Array(gridWidth * gridHeight),
//      queue = [],
//      n = 0,
//      count = -1;
//        }

//var context = canvas.node().getContext("2d");
//context.lineWidth = .5 * ratio;
//
//reset(2 * radius, width / 2, height / 2);
//
//function reset(r, x, y) {
//  context.clearRect(0, 0, width, height);
//
//  var id = ++active,
//      inner2 = r * r,
//      A = 4 * r * r - inner2,
//      cellSize = r * Math.SQRT1_2,
//      gridWidth = Math.ceil(width / cellSize),
//      gridHeight = Math.ceil(height / cellSize),
//      grid = new Array(gridWidth * gridHeight),
//      queue = [],
//      n = 0,
//      count = -1;
//
//  emitSample([x, y]);
//  d3.timer(function() {
//    if (id !== active) return true;
//
//    var start = Date.now();
//    do {
//      var i = Math.random() * n | 0,
//          p = queue[i];
//
//      for (var j = 0; j < k; ++j) {
//        var q = generateAround(p);
//        if (withinExtent(q) && !near(q)) {
//          emitSample(q);
//          break;
//        }
//      }
//      // No suitable candidate found; remove from active queue.
//      if (j === k) queue[i] = queue[--n], queue.pop();
//    } while (n && Date.now() - start < 17);
//    return !n;
//  });
//
//  function emitSample(p) {
//    queue.push(p), ++n;
//    grid[gridWidth * (p[1] / cellSize | 0) + (p[0] / cellSize | 0)] = p;
//
//    context.beginPath();
//    context.arc(p[0], p[1], ratio, 0, 2 * Math.PI);
//    context.stroke();
//  }
//
//  // Generate point chosen uniformly from spherical annulus between radius r
//  // and 2r from p.
//  function generateAround(p) {
//    var θ = Math.random() * 2 * Math.PI,
//        r = Math.sqrt(Math.random() * A + inner2); // http://stackoverflow.com/a/9048443/64009
//    return [p[0] + r * Math.cos(θ), p[1] + r * Math.sin(θ)];
//  }
//
//  function near(p) {
//    var n = 2,
//        x = p[0] / cellSize | 0,
//        y = p[1] / cellSize | 0,
//        x0 = Math.max(x - n, 0),
//        y0 = Math.max(y - n, 0),
//        x1 = Math.min(x + n + 1, gridWidth),
//        y1 = Math.min(y + n + 1, gridHeight);
//    for (var y = y0; y < y1; ++y) {
//      var o = y * gridWidth;
//      for (var x = x0; x < x1; ++x) {
//        var g = grid[o + x];
//        if (g && distance2(g, p) < inner2) return true;
//      }
//    }
//    return false;
//  }
//}
//
//function withinExtent(p) {
//  var x = p[0], y = p[1];
//  return x0 <= x && x <= x1 && y0 <= y && y <= y1;
//}
//
//function distance2(a, b) {
//  var dx = b[0] - a[0],
//      dy = b[1] - a[1];
//  return dx * dx + dy * dy;
//}
    }
}
