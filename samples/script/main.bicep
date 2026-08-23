targetScope = 'local'

extension utilities

param name string
param platform 'Bash' | 'PowerShell'

resource sayHelloWithBash 'Script' = if (platform == 'Bash') {
  type: 'Bash'
  script: replace(loadTextContent('./script.sh'), '$INPUT_NAME', name)
}

resource sayHelloWithPowerShell 'Script' = if (platform == 'PowerShell') {
  type: 'PowerShell'
  script: replace(loadTextContent('./script.ps1'), '$INPUT_NAME', name)
}

output stdout string? = (platform == 'Bash') ? sayHelloWithBash.?stdOut : sayHelloWithPowerShell.?stdOut
