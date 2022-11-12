const colors = require('tailwindcss/colors')

module.exports = {
  content: [
    "./**/*.{razor,html,cshtml,razor.css}",
    './Styles/**/*.{pcss,css}',
    './Components/**/*.cs'
  ],
  safelist: [
    {
      pattern: /-{0,1}(p|m|w|h|px|py|pl|pr|pt|pb|mx|my|ml|mr|mt|mb|gap)-[0-9]{1,3}/
    },
  ],
  theme: {
    extend: {
      borderRadius: {
        'xs': '0.0625rem',
      },
      spacing: {
        '104': '26rem',
        '112': '28rem',
        '120': '30rem',
        '128': '32rem',
        '136': '34rem',
        '144': '36rem',
        '152': '38rem',
        '160': '40rem',
      },
      colors: {
        positive: colors.teal,
        negative: colors.rose,
        neutral: colors.blue
      },
    },
  },
  plugins: [
    require('@tailwindcss/typography')
  ],
}