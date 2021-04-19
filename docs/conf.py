# Configuration file for the Sphinx documentation builder.
#
# This file only contains a selection of the most common options. For a full
# list see the documentation:
# https://www.sphinx-doc.org/en/master/usage/configuration.html

# -- Path setup --------------------------------------------------------------

# If extensions (or modules to document with autodoc) are in another directory,
# add these directories to sys.path here. If the directory is relative to the
# documentation root, use os.path.abspath to make it absolute, like shown here.

import os
import sys
sys.path.insert(0, os.path.abspath('./extensions'))

# -- Project information -----------------------------------------------------

# https://www.sphinx-doc.org/en/master/usage/configuration.html#project-information

project = "Crest"
author = "Wave Harmonic & Contributors"
copyright = f"2021, {author}"
version = "4.11"
# https://www.sphinx-doc.org/en/master/usage/configuration.html#confval-release
release = version

# -- General configuration ---------------------------------------------------

# Add any Sphinx extension module names here, as strings. They can be
# extensions coming with Sphinx (named 'sphinx.ext.*') or your custom
# ones.
extensions = [
    "sphinx_inline_tabs",
    "sphinx_panels",
    "sphinx_issues",

    # For using CONTRIBUTING.md.
    "sphinx_markdown_tables",
    "recommonmark",

    # Theme.
    "furo",

    # Local packages.
    "youtube",
    "variables",
    "tags",
    "links",
    "hacks",

    "notfound.extension",

    # These extensions require RTDs to work so they will not work locally.
    "hoverxref.extension",
    "sphinx_search.extension",
]

# Add any paths that contain templates here, relative to this directory.
templates_path = ['_templates']

# List of patterns, relative to source directory, that match files and
# directories to ignore when looking for source files.
# This pattern also affects html_static_path and html_extra_path.
exclude_patterns = [
    '_build',
    'Thumbs.db',
    '.DS_Store',
    ".env",
    "extensions",
    "**/includes",
    "README.md",
]

# https://github.com/readthedocs/readthedocs.org/issues/4603
if os.environ.get('PLATFORM') == "READTHEDOCS":
    tags.add('readthedocs')
    tags.add("birp")
    tags.add("hdrp")
    tags.add("urp")
else:
    notfound_no_urls_prefix = True

# -- Features ----------------------------------------------------------------

# Auto numbering of figures
numfig = True

# GitHub repo
issues_github_path = "wave-harmonic/crest"

# https://sphinx-hoverxref.readthedocs.io/en/latest/usage.html#tooltip-on-all-ref-roles
hoverxref_auto_ref = True

# -- Options for HTML output -------------------------------------------------

# The theme to use for HTML and HTML Help pages.  See the documentation for
# a list of builtin themes.
# https://github.com/pradyunsg/furo
# https://pradyunsg.me/furo/
html_theme = 'furo'
html_title = "Crest Ocean System"
html_short_title = "Crest"
# html_logo = '../logo/crest-oceanrender-logo.svg'
html_favicon = '../logo/crest-oceanrender-logomark.png'

html_theme_options = {
    "light_logo": "crest-oceanrender-logo.svg",
    "dark_logo": "crest-oceanrender-logo-dark.svg",
    "sidebar_hide_name": True,
    # "announcement": "<em>Important</em> announcement!",
}

html_show_sphinx = False

# Add any paths that contain custom static files (such as style sheets) here,
# relative to this directory. They are copied after the builtin static files,
# so a file named "default.css" will overwrite the builtin "default.css".
html_static_path = ["_static", "../logo"]

# These paths are either relative to html_static_path or fully qualified paths (eg. https://...).
# Increment query parameter to invalidate the cache.
html_css_files = [
    'custom.css?v1.1.0',
]

html_js_files = [
    'https://cdnjs.cloudflare.com/ajax/libs/medium-zoom/1.0.6/medium-zoom.min.js',
    'custom.js?v1.2.0',
]

# -- Options for PDF output --------------------------------------------------

# Customise PDF here. maketitle overrides the cover page.
latex_elements = {
    # "maketitle": "\\input{your_cover.tex}"
    # "maketitle": "\\sphinxmaketitle",
}

latex_logo = "../logo/crest-oceanrender-logomark512.png"

# -- Templating --------------------------------------------------------------

# The default role will be used for `` so we do not need to do :get:``.
default_role = "get"

# "replace" substitutions are static/global:
#   |name1| replace:: value
#   |name1|
# Cannot do this:
#   |name2| replace:: |name1|
# Inline content has no nested parsing.

# "set" only supports inline content. It will pass its contents to the parser so roles will be processed. Brace
# substitution is supported and is text only (it will lose any nodes). Use it when you need substitutions in role
# content.
#   .. set:: LongName Example
#   .. set:: ShortName :abbr:`{LongName}`
#   An example of using `ShortName`.

# For links where you want to use substitutions, use the link role:
#   .. set Something Example Page
#   .. set BaseURL https://example.com
#   :link:`Link Text for {Something} <{BaseURL}/example>`
# Pass the URL within the angle brackets. Brace substitution will work and will be text only for URLs and support nodes
# for the link text.
#
# For URLs, it is best to use braces even in "set" as they don't require being enclosed in escaped whitespace:
#   .. set:: Link `LinkBase`\ /something/\ `LinkPart`\ /example.html
# Versus:
#   .. set:: Link {LinkBase}/something/{LinkPart}/example.html

# The following will be included before every page:
rst_prolog = f"""
.. tags::
.. set:: AssetVersion {version}
"""
rst_prolog = rst_prolog + """
.. set:: RPMinVersion 7.6
.. set:: RPDocLinkBase \https://docs.unity3d.com/Packages/com.unity.render-pipelines.
.. set:: UnityMinVersionShort 2019.4
.. set:: UnityMinVersion {UnityMinVersionShort}.24
.. set:: UnityDocLink https://docs.unity3d.com/{UnityMinVersionShort}/Documentation/Manual
.. set:: UnityIssueLink https://issuetracker.unity3d.com/product/unity/issues/guid
.. set:: AssetStoreLinkBase \https://assetstore.unity.com/packages/tools/particles-effects
.. set:: DocLinkBase https://crest.readthedocs.io/en
.. set:: GitHubLink \https://github.com/wave-harmonic/crest
.. set:: WikiLink \{GitHubLink}/wiki

.. set:: [BIRP] :guilabel:`BIRP`
.. set:: BIRPNameLong Built-in
.. set:: BIRPNameShort BIRP
.. set:: BIRPNameSlug birp
.. set:: BIRP :abbr:`{BIRPNameShort} ({BIRPNameLong} Render Pipeline)`
.. set:: BIRPMinVersion `RPMinVersion`
.. set:: BIRPDocLink {UnityDocLink}/
.. set:: BIRPAssetDocLink {DocLinkBase}/{AssetVersion}?rp={BIRPNameSlug}

.. set:: [URP] :guilabel:`URP`
.. set:: URPNameLong Universal
.. set:: URPNameShort URP
.. set:: URPNameSlug urp
.. set:: URP :abbr:`{URPNameShort} ({URPNameLong} Render Pipeline)`
.. set:: URPMinVersion `RPMinVersion`
.. set:: URPDocLink {RPDocLinkBase}universal@{URPMinVersion}/manual
.. set:: URPAssetLink {AssetStoreLinkBase}/crest-ocean-system-urp-141674
.. set:: URPAssetDocLink {DocLinkBase}/{AssetVersion}?rp={URPNameSlug}

.. set:: [HDRP] :guilabel:`HDRP`
.. set:: HDRPNameLong High Definition
.. set:: HDRPNameShort HDRP
.. set:: HDRPNameSlug hdrp
.. set:: HDRP :abbr:`{HDRPNameShort} ({HDRPNameLong} Render Pipeline)`
.. set:: HDRPMinVersion `RPMinVersion`
.. set:: HDRPDocLink {RPDocLinkBase}high-definition@{HDRPMinVersion}/manual
.. set:: HDRPAssetLink {AssetStoreLinkBase}/crest-ocean-system-hdrp-164158
.. set:: HDRPAssetDocLink {DocLinkBase}/{AssetVersion}?rp={HDRPNameSlug}

.. set:: Crest *Crest*

.. set:: TAA :abbr:`TAA (Temporal Anti-Aliasing)`
.. set:: SMAA :abbr:`SMAA (Subpixel Morphological Anti-Aliasing)`
.. set:: SPI :abbr:`SPI (Single Pass Instanced)`

.. set:: DWP2 :abbr:`DWP2 (Dynamic Water Physics 2)`
"""

# -- Debugging ---------------------------------------------------------------

# For debugging if you want to always have a tag on or off
# tags.add("tag")
# tags.remove("tag")
