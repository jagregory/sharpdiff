require 'albacore'

task :default => :build

desc 'Build SharpDiff and run all tests'
task :build => [:compile, :run_tests]

desc 'Compile the source'
msbuild :compile do |msb|
  msb.properties = { :configuration => :Release }
  msb.targets [:Clean, :Build]
  msb.solution = 'src/SharpDiff.sln'
end

desc 'Run unit tests'
nunit :run_tests do |nunit|
  nunit.path_to_command = 'lib/nunit/nunit-console.exe'
  nunit.assemblies << 'src/SharpDiff.Tests/bin/Release/SharpDiff.Tests.dll'
end

desc 'Rebuild the parsers'
task :rebuild_parsers do
  `tools/rebuildParser/RebuildParser.exe`
end

namespace :setup do
  task :ensure_gemcutter_source do
    puts 'Ensuring gemcutter.org is registered as a gem source'
    unless `gem source -l`.include? 'http://gemcutter.org'
      puts 'Setting Gemcutter as a gem source'
      `gem source -a http://gemcutter.org`
    end
  end
end
