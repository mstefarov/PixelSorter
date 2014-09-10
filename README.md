PixelSorter
===========
Sort pixels in your images in artistic ways.
![](http://i.imgur.com/hkZACBk.png)

Requires .NET Framework 3.5 (Client Profile will suffice). Project files are compatible with VisualStudio 2010 or newer.

Download
========
**[PixelSorter_r17_NET35.exe](http://matvei.org/pixelsorter/PixelSorter_r17_NET35.exe)**
(for .NET 3.5+, 31 KiB)<br>
SHA-1 checksum: <code>2851ee552c28e687d304210da0e3de993838f1ba</code>

**[PixelSorter_r17_NET40.exe](http://matvei.org/pixelsorter/PixelSorter_r17_NET40.exe)**
(for .NET 4.0+, 32 KiB)<br>
SHA-1 checksum: <code>fe1e55d5190a2b39ec5e98f7b748db6fde67ff07</code>

Examples
========
![](http://i.imgur.com/o7c9qma.png)
![](http://i.imgur.com/obJyUau.png)
![](http://i.imgur.com/o51vIYA.png)
![](http://i.imgur.com/cKHOeEK.png)

Parameter Explanation
=====================
### Algorithms
Determines the sorting pattern.
- *Whole Image* - All segments are sorted row-by-row, top-down, left-to-right.
- *Row* - Segments within each row are sorted left-to-right.
- *Column* - Segments within each column are sorted top-to-bottom.
- *Segment* - All pixels within each segment are sorted row-by-row, top-down, left-to-right. Segments are not re-arranged.

### Order
Determines sorting order.
- *Ascending* - Segments with lowest value come first.
- *Descending* - Segments with highest value come first.
- *Ascending Reflected* - Segments with lowest values are in the center.
- *Descending Reflected* - Segments with highest values are in the center.
- *Ascending Thresholded* - Each row/column is divided into regions, divided based on difference between adjacent segments. Segments are sorted lowest-value-first within each region. Only works with *Row* or *Column* algorithm.
- *Descending Thresholded* - Same as *Ascending Thresholded*, but segments are sorted highest-value-first within each region.
- *Random* - Segments are arranged randomly. If algorithm is *Segment*, pixels within each segment are arranged randomly instead.

### Metric
Determines how the "value" of a single pixel is measured.
- *Intensity* - <code>(R+G+B)/3</code> (HSI color space)
- *Lightness* - <code>(max(R,G,B) + min(R,G,B))/2</code> (HSL color space)
- *Value* - <code>max(R,G,B)</code> (HSV color space)
- *Luma* - <code>R*0.2126 + G*0.7152 + B*0.0722</code> (Approximate luminosity per [Rec. 709](https://en.wikipedia.org/wiki/Luma_(video)))
- *Hue (HSL)* - Approximate Hue, HSL color space. Follows the rainbow order (ROYGBIV).
- *Hue (Lab)* - Accurate Hue, [CIE 1976 Lab color space](https://en.wikipedia.org/wiki/Lab_color_space#CIELAB).
- *Chroma (HSL)* - <code>max(R,G,B) - min(R,G,B)</code>. Approximate color intensity (HSL color space)
- *Chroma (Lab)* - Accurate color intensity, CIE 1976 Lab color space.
- *Saturation (HSB)* - <code>(max(R,G,B) - min(R,G,B))/max(R,G,B)</code> Approximate color saturation (HSB color space)
- *Saturation (HSI)* - <code>1 - min(R,G,B)/((R+G+B)/3)</code> Approximate color saturation (HSI color space)
- *Saturation (HSL)* - Approximate color saturation (HSL color space)
- *Saturation (Lab)* - <code>sqrt(a²+b²)/L</code> Accurate color saturation, CIE 1976 Lab color space.
- *Red Channel* - Raw red component of RGB color.
- *Green Channel* - Raw green component of RGB color.
- *Blue Channel* - Raw blue component of RGB color.
- *Red* - <code>R - max(G,B)</code> Dominance of color red.
- *Green* - <code>G - max(R,B)</code> Dominance of color green.
- *Blue* - <code>B - max(R,G)</code> Dominance of color blue.
- *Cyan* - <code>(G+B) - max(R*1.5, |G-B|)</code> Dominance of color cyan (or absence of red)
- *Magenta* - <code>(R+B) - max(G*1.5, |R-B|)</code> Dominance of color magenta (or absence of green).
- *Yellow* - <code>(R+G) - max(B*1.5, |R-G|)</code> Dominance of color yellow (or absence of blue).
- *LabA*  - Green-magenta balance. The **a\*** component of CIE 1976 Lab color space.
- *LabB*  - Blue-yellow balance. The **b\*** component of CIE 1976 Lab color space.

### Sampling
Determines how the total "value" of a multi-pixel segment is measured.
- *Center* - Only value of the middle pixel is measured. Fast. Inaccurate for detailed images and large segments.
- *Average (Mean)* - Values of all pixels in each segment are added up.
- *Average (Median)* - Values of "an average pixel", such that about half of the segment have same or lower value, and other half has same or higher value.
- *Maximum* - Highest value of all pixels in a segment.
- *Minimum* - Lowest value of all pixels in a segment.
- *Random* - Value of a random pixel within the segment is chosen.
